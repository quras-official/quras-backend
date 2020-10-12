using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Quras.Core;
using Quras.IO.Json;

namespace Quras
{
    class BackendService
    {
        private static bool IsRunning = false;
        private DatabaseModel DBModel = null;
        private RPCModel RPCModel = null;

        private TcpListener server = null;

        public BackendService()
        {

        }
        public void Run()
        {
            OnStart();
            ServiceRun();
            OnStop();
        }

        public void ProcessBlockThread()
        {
            
            while (IsRunning)
            {
                try
                {
                    DBModel.TransactionStart();

                    int db_block_num = DBModel.GetDatabaseBlockNumber();
                    if (db_block_num == -1) throw new Exception("Getting database block number failed.");

                    LogUtils.Default.Log("Current DB Block Number : " + db_block_num);

                    int current_block_num = RPCModel.GetBlockchainNumber();
                    if (current_block_num == -1)
                    {
                        DBModel.RollBack();
                        Thread.Sleep(Constants.NORMAL_DELAY);
                        continue;
                    }

                    if (db_block_num >= current_block_num)
                    {
                        DBModel.RollBack();
                        Thread.Sleep(Constants.NORMAL_DELAY);
                        continue;
                    }

                    Block block = RPCModel.GetBlock(db_block_num);

                    if (block == null)
                    {
                        DBModel.RollBack();
                        Thread.Sleep(Constants.NORMAL_DELAY);
                        continue;
                    }

                    if (DBModel.RegBlockDataOnDB(block) == false) throw new Exception("RegBlockDataOnDB function failed.");

                    for (int i = 0; i < block.TxCount; i++)
                    {
                        JObject jtx = JObject.Parse(block.Txs[i]);
                        Transaction tx = new Transaction();

                        tx.FromJson(jtx);
                        if (DBModel.RegTransactionDataOnDB(block, tx) == false) throw new Exception("RegTransactionDataOnDB function failed.");

                        switch (tx.Type)
                        {
                            case "IssueTransaction":
                                IssueTransaction itx = new IssueTransaction();
                                itx.FromJson(jtx);
                                if (DBModel.RegIssueTransactionDataOnDB(block, itx) == false) throw new Exception("RegIssueTransactionDataOnDB function failed.");
                                if (DBModel.IssueHoldersOnDB(block, itx) == false) throw new Exception("IssueHoldersOnDB failed");
                                break;
                            case "ClaimTransaction":
                                ClaimTransaction ctx = new ClaimTransaction();
                                ctx.FromJson(jtx);
                                if (DBModel.RegClaimTransactionDataOnDB(block, ctx) == false) throw new Exception("RegClaimTransactionDataOnDB function failed.");
                                if (DBModel.ClaimTransactionToUtxosStatusOnDB(block, ctx) == false) throw new Exception("ClaimTransactionToUtxosStatusOnDB function failed.");
                                break;
                            case "EnrollmentTransaction":
                                EnrollmentTransaction etx = new EnrollmentTransaction();
                                etx.FromJson(jtx);
                                if (DBModel.EnrollmentTransactionOnDB(block, etx) == false) throw new Exception("EnrollmentTransactionOnDB function failed.");
                                break;
                            case "RegisterTransaction":
                                RegisterTransaction rtx = new RegisterTransaction();
                                rtx.FromJson(jtx);
                                if (DBModel.RegRegisterTransactionDataOnDB(block, rtx) == false) throw new Exception("RegRegisterTransactionDataOnDB function failed.");
                                break;
                            case "ContractTransaction":
                                ContractTransaction contractTx = new ContractTransaction();
                                contractTx.FromJson(jtx);
                                if (DBModel.ContractTransactionOnDB(block, contractTx) == false) throw new Exception("ContractTransactionOnDB function failed.");
                                if (DBModel.ContractHoldersOnDB(block, contractTx) == false) throw new Exception("ContractHoldersOnDB function failed.");
                                break;
                            case "AnonymousContractTransaction":
                                AnonymousContractTransaction atx = new AnonymousContractTransaction();
                                atx.FromJson(jtx);
                                if (DBModel.AnonymousContractTransactionOnDB(block, atx) == false) throw new Exception("AnonymousContractTransactionOnDB function failed.");
                                break;
                            case "RingConfidentialTransaction":
                                RingConfidentialTransaction ringTx = new RingConfidentialTransaction();
                                ringTx.FromJson(jtx);
                                if (DBModel.RingConfidentTransactionOnDB(block, ringTx) == false) throw new Exception("RingConfidentTransactionOnDB function failed.");
                                break;
                            case "PublishTransaction":
                                PublishTransaction ptx = new PublishTransaction();
                                ptx.FromJson(jtx);
                                if (DBModel.PublishTransactionOnDB(block, ptx) == false) throw new Exception("PublishTransactionOnDB function failed.");
                                break;
                            case "InvocationTransaction":
                                InvocationTransaction invocTx = new InvocationTransaction();
                                invocTx.FromJson(jtx);
                                if (DBModel.InvocationTransactionOnDB(block, invocTx) == false) throw new Exception("InvocationTransactionOnDB function failed.");
                                if (DBModel.RegAssetTransactionOnDB(block, invocTx) == false) throw new Exception("RegAssetTransactionOnDB function failed.");
                                break;
                            case "MinerTransaction":
                                MinerTransaction mtx = new MinerTransaction();
                                mtx.FromJson(jtx);
                                if (DBModel.MinerTransactionOnDB(block, mtx) == false) throw new Exception("MinerTransactionOnDB function failed.");
                                break;
                            case "StorageTransaction":
                                StorageTransaction stx = new StorageTransaction();
                                stx.FromJson(jtx);
                                if (DBModel.RegStorageWalletTransaction(block, stx) == false) throw new Exception("RegStorageWalletTransaction function failed.");
                                break;
                            case "UploadRequestTransaction":
                                UploadRequestTransaction utx = new UploadRequestTransaction();
                                utx.FromJson(jtx);
                                if (DBModel.RegUploadRequestTransaction(block, utx) == false) throw new Exception("RegUploadRequestTransaction function failed.");
                                break;
                            case "PayUploadTransaction":
                                PayUploadTransaction payTx = new PayUploadTransaction();
                                payTx.FromJson(jtx);
                                if (DBModel.RegPayUploadTransaction(block, payTx) == false) throw new Exception("RegPayUploadTransaction function failed.");
                                break;
                            case "DownloadRequestTransaction":
                                DownloadRequestTransaction dtx = new DownloadRequestTransaction();
                                dtx.FromJson(jtx);
                                if (DBModel.RegDownloadRequestTransactionOnDB(block, dtx) == false) throw new Exception("RegDownloadRequestTransactionOnDB function failed.");
                                break;
                            case "ApproveDownloadTransaction":
                                ApproveDownloadTransaction approveTx = new ApproveDownloadTransaction();
                                approveTx.FromJson(jtx);
                                if (DBModel.RegApproveDownloadTransactionDataOnDB(block, approveTx) == false) throw new Exception("RegApproveDownloadTransactionDataOnDB function failed.");
                                break;
                            case "PayFileTransaction":
                                PayFileTransaction payFileTx = new PayFileTransaction();
                                payFileTx.FromJson(jtx);
                                if (DBModel.RegPayFileTransactionDataOnDB(block, payFileTx) == false) throw new Exception("RegPayFileTransactionDataOnDB function failed.");
                                break;
                            case "StateTransaction":
                                StateTransaction stateTx = new StateTransaction();
                                stateTx.FromJson(jtx);
                                if (DBModel.RegStateTransactionDataOnDB(block, stateTx) == false) throw new Exception("RegStateTransactionDataOnDB function failed");
                                if (DBModel.UpdatePermissionState(block, stateTx) == false) throw new Exception("UpdatePermissionState function failed");
                                if (DBModel.UpdateStorageState(block, stateTx) == false) throw new Exception("UpdateStorageState function failed");
                                break;
                            case "RegisterMultiSignTransaction":
                                RegisterMultiSignTransaction multiSigAddressTx = new RegisterMultiSignTransaction();
                                multiSigAddressTx.FromJson(jtx);
                                if (DBModel.RegisterMultiSignTransactionOnDB(block, multiSigAddressTx) == false) throw new Exception("RegisterMultiSignTransactionOnDB function failed.");
                                break;
                        }

                        if (DBModel.AddUtxosOnDB(block, tx) == false) throw new Exception("AddUtxosOnDB function failed");
                        if (DBModel.UpdateUtxosStatusOnDB(block, tx) == false) throw new Exception("UpdateUtxosStatusOnDB function failed");
                    }

                    if (DBModel.UpdateTaskId(block, db_block_num) == false) throw new Exception("UpdateTaskId function failed");

                    DBModel.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.Default.Log(ex.ToString());

                    DBModel.RollBack();
                    Thread.Sleep(Constants.FAST_DELAY);
                }
                //Thread.Sleep(Constants.FAST_DELAY);
            }
        }

        public void ServiceRun()
        {
            Thread processStart = new Thread(StartServer);
            processStart.Start();

            Thread processBlock = new Thread(ProcessBlockThread);
            processBlock.Start();
            while(IsRunning)
            {
                Thread.Sleep(Constants.NORMAL_DELAY);
            }
        }
        private void OnStart()
        {
            DBModel = new DatabaseModel();
            DatabaseModel.instance = DBModel;
            RPCModel = new RPCModel();
            IsRunning = true;
        }
        private void OnStop()
        {
            
        }

        private void StartServer()
        {
            
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 10000;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(port);
                server.Start();

                // Start listening for client requests.
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            while (true)
            {
                Thread.Sleep(Constants.NORMAL_DELAY);
            }
        }

        public void StartListen()
        {
            while (true)
            {
                Socket mySocket = server.AcceptSocket();

                if (mySocket.Connected)
                {
                    string ret = "OK";
                    mySocket.Send(Encoding.ASCII.GetBytes("OK"));
                }
            }
        }
    }
}
