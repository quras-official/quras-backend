using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Quras.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Quras.Constants;

namespace Quras
{
    internal sealed partial class DatabaseModel
    {
        private string SQL_INSERT_BLOCK = "REPLACE INTO blocks (hash, size, version, prev_block_hash, merkle_root, time, block_number, nonce, current_consensus, next_consensus, script, tx_count) VALUES (@hash, @size, @version, @prevhash, @merkleroot, @time, @blocknum, @nonce, @currentconsensus, @nextconsensus, @script, @txcount)";
        private string SQL_INSERT_TRANSACTION = "REPLACE INTO transactions (txid, size, tx_type, version, attribute, vin, vout, sys_fee, net_fee, scripts, nonce, time, block_number, hash) VALUES (@txid, @size, @tx_type, @version, @attribute, @vin, @vout, @sys_fee, @net_fee, @scripts, @nonce, @time, @block_number, @hash)";
        private string SQL_INSERT_REGISTER_TRANSACTION = "REPLACE INTO register_transaction (txid, type, name, amount, _precision, owner, admin, tfee_min, tfee_max, afee, time, block_number, hash) VALUES (@txid, @type, @name, @amount, @_precision, @owner, @admin, @tfee_min, @tfee_max, @afee, @time, @block_number, @hash)";
        private string SQL_INSERT_ISSUE_TRANSACTION = "REPLACE INTO issue_transaction (txid, address, asset, name, _to, amount, fee, time, block_number, hash) VALUES (@txid, @address, @asset, @name, @_to, @amount, @fee, @time, @block_number, @hash)";
        private string SQL_INSERT_ISSUE_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, issue_transaction_address, issue_transaction_asset, issue_transaction_to, issue_transaction_amount, issue_transaction_fee, name) VALUES (@txid, @time, @block_number, @hash, @tx_type, @issue_transaction_address, @issue_transaction_asset, @issue_transaction_to, @issue_transaction_amount, @issue_transaction_fee, @name)";
        private string SQL_INSERT_CLAIM_TRANSACTION = "REPLACE INTO claim_transaction (txid, claims, address, amount, time, block_number, hash) VALUES (@txid, @claims, @address, @amount, @time, @block_number, @hash)";
        private string SQL_INSERT_CLAIM_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, claim_transaction_address, claim_transaction_amount, name) VALUES (@txid, @time, @block_number, @hash, @tx_type, @claim_transaction_address, @claim_transaction_amount, @name)";
        private string SQL_INSERT_ENROLLMENT_TRANSACTION = "REPLACE INTO enrollment_transaction (txid, pubkey) VALUES (@txid, @pubkey)";
        private string SQL_INSERT_CONTRACT_TRANSACTION = "REPLACE INTO contract_transaction (txid, _from, _to, asset, name, amount, fee, time, block_number, hash) VALUES (@txid, @_from, @_to, @asset, @name, @amount, @fee, @time, @block_number, @hash)";
        private string SQL_INSERT_CONTRACT_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, contract_transaction_from, contract_transaction_to, contract_transaction_asset, contract_transaction_amount, contract_transaction_fee, name) VALUES (@txid, @time, @block_number, @hash, @tx_type, @contract_transaction_from, @contract_transaction_to, @contract_transaction_asset, @contract_transaction_amount, @contract_transaction_fee, @name)";
        private string SQL_INSERT_HOLDERS = "REPLACE INTO holders (address, name, asset, balance) VALUES (@address, @name, @asset, @balance)";
        private string SQL_UPDATE_FROM_HOLDER = "UPDATE holders SET balance=balance-@amount WHERE address=@address AND asset=@asset";
        private string SQL_UPDATE_TO_HOLDER = "UPDATE holders SET balance=balance+@amount WHERE address=@address AND asset=@asset";
        private string SQL_INSERT_ANONYMOUS_TRANSACTION = "REPLACE INTO anonymous_contract_transaction (txid, time, block_number, hash) VALUES (@txid, @time, @block_number, @hash)";
        private string SQL_INSERT_ANONYMOUS_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type) VALUES (@txid, @time, @block_number, @hash, @tx_type)";
        private string SQL_INSERT_RING_CONFIDENT_TRANSACTION = "REPLACE INTO ring_confident_transaction (txid, time, block_number, hash) VALUES (@txid, @time, @block_number, @hash)";
        private string SQL_INSERT_RING_CONFIDENT_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type) VALUES (@txid, @time, @block_number, @hash, @tx_type)";
        private string SQL_INSERT_PUBLISH_TRANSACTION = "REPLACE INTO publish_transaction (txid, contract_code_hash, contract_code_script, contract_code_parameters, contract_code_returntype, contract_needstorage, contract_name, contract_version, contract_author, contract_email, contract_description) VALUES (@txid, @contract_code_hash, @contract_code_script, @contract_code_parameters, @contract_code_returntype, @contract_needstorage, @contract_name, @contract_version, @contract_author, @contract_email, @contract_description)";
        private string SQL_INSERT_INVOCATION_TRANSACTION = "REPLACE INTO invocation_transaction (txid, address, script, gas, time, block_number, hash) VALUES (@txid, @address, @script, @gas, @time, @block_number, @hash)";
        private string SQL_INSERT_INVOCATION_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, invocation_transaction_address, gas, name) VALUES (@txid, @time, @block_number, @hash, @tx_type, @invocation_transaction_address, @gas, @name)";
        private string SQL_INSERT_MINER_TRANSACTION = "REPLACE INTO miner_transaction (txid, address, _to, amount, nonce, time, block_number, hash) VALUES (@txid, @address, @_to, @amount, @nonce, @time, @block_number, @hash)";
        private string SQL_INSERT_MINER_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, miner_transaction_address, miner_transaction_to, miner_transaction_amount, name) VALUES (@txid, @time, @block_number, @hash, @tx_type, @miner_transaction_address, @miner_transaction_to, @miner_transaction_amount, @name)";


        private string SQL_INSERT_STATE_TRANSACTION = "REPLACE INTO state_transaction (txid, from_address, to_address, upload_hash, state_script, time, block_number, hash) VALUES (@txid, @fromaddr, @toaddr, @uploadhash, @statescript, @time, @blocknum, @hash)";
        private string SQL_INSERT_STATE_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, state_from_address, state_to_address, state_upload_hash, state_state_script) VALUES (@txid, @time, @blocknum, @hash, @txtype, @fromaddr, @toaddr, @uploadhash, @statescript)";
        private string SQL_INSERT_PAYFILE_TRANSACTION = "REPLACE INTO pay_file_transaction (txid, download_address, upload_address, asset, name, amount, fee, time, block_number, hash, dtx_hash) VALUES (@txid, @downaddr, @upaddr, @asset, @name, @amount, @fee, @time, @blocknum, @hash, @dtxhash)";
        private string SQL_INSERT_PAYFILE_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, payfile_transaction_download_address, payfile_transaction_upload_address, payfile_transaction_asset, payfile_transaction_name, payfile_transaction_pay_amount, payfile_transaction_fee, payfile_transaction_tx_hash) VALUES (@txid, @time, @blocknum, @hash, @txtype, @downaddr, @upaddr, @asset, @name, @amount, @fee, @dtxhash)";
        private string SQL_INSERT_APPROVEDOWNLOAD_TRANSACTION = "REPLACE INTO approve_download_transaction (txid, approve_address, download_address, dtx_hash, approve_state, time, block_number, hash) VALUES (@txid, @approvaddr, @downaddr, @dtxhash, @approvstate, @time, @blocknum, @hash)";
        private string SQL_INSERT_APPROVEDOWNLOAD_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, approvedownload_transaction_approve_address, approvedownload_transaction_download_address, approvedownload_transaction_tx_hash, approvedownload_transaction_approve_state) VALUES (@txid, @time, @blocknum, @hash, @txtype, @approvaddr, @downaddr, @dtxhash, @approvstate)";
        private string SQL_INSERT_DOWNLOADREQUEST_TRANSACTION = "REPLACE INTO download_request_transaction (txid, file_name, file_description, file_url, pay_amount, upload_address, download_address, file_hash, time, block_number, hash) VALUES (@txid, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @downloadaddr, @filehash, @time, @blocknum, @hash)";
        private string SQL_INSERT_DOWNLOADREQUEST_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, downloadrequest_transaction_file_name, downloadrequest_transaction_file_description, downloadrequest_transaction_file_url, downloadrequest_transaction_pay_amount, downloadrequest_transaction_upload_address, downloadrequest_transaction_download_address, downloadrequest_transaction_tx_hash) VALUES (@txid, @time, @blocknum, @hash, @txtype, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @downloadaddr, @filehash)";
		private string SQL_INSERT_PAYUPLOAD_TRANSACTION = "REPLACE INTO pay_upload_transaction (txid, utxid, upload_size, from_address, to_address, asset, name, amount, fee, time, block_number, hash) VALUES (@txid, @utxid, @uploadsize, @fromaddr, @toaddr, @asset, @name, @amount, @fee, @time, @blocknum, @hash)";
		private string SQL_INSERT_PAYUPLOAD_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, payupload_transaction_utxid, payupload_transaction_size, payupload_transaction_from, payupload_transaction_to, payupload_transaction_asset, payupload_transaction_name, payupload_transaction_amount, payupload_transaction_fee) VALUES (@txid, @time, @blocknum, @hash, @txtype, @utxid, @uploadsize, @fromaddr, @toaddr, @asset, @name, @amount, @fee)";
		private string SQL_INSERT_UPLOADREQUEST_TRANSACTION = "REPLACE INTO upload_request_transaction (txid, file_name, file_description, file_url, pay_amount, upload_address, file_verifiers, duration_days, time, block_number, hash) VALUES (@txid, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @fileverifiers, @durationdays, @time, @blocknum, @hash)";
		private string SQL_INSERT_UPLOADREQUEST_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, uploadrequest_transaction_file_name, uploadrequest_transaction_file_description, uploadrequest_transaction_file_url, uploadrequest_transaction_pay_amount, uploadrequest_transaction_upload_address, uploadrequest_transaction_verifiers, uploadrequest_transaction_duration_days) VALUES (@txid, @time, @blocknum, @hash, @txtype, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @fileverifiers, @durationdays)";
		private string SQL_INSERT_STORAGEWALLET_TRANSACTION = "REPLACE INTO storage_wallets (txid, address, storage_size, gurantee_amount_per_gb, pay_amount_per_gb, end_time) VALUES (@txid, @address, @storagesize, @guarantee, @payamount, @endtime)";
		private string SQL_INSERT_STORAGEWALLET_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, storage_wallet_address, storage_wallet_size, storage_wallet_gurantee_amount_per_gb, storage_wallet_pay_amount_per_gb, storage_wallet_end_tim) VALUES (@txid, @time, @blocknum, @hash, @txtype, @address, @storagesize, @guarantee, @payamount, @endtime)";
		private string SQL_INSERT_UTXO = "REPLACE INTO utxos (txid, tx_out_index, asset, name, value, address, status, time, block_number, hash) VALUES (@txid, @txoutindex, @asset, @name, @value, @address, @status, @time, @blocknum, @hash)";
		private string SQL_INSERT_APPROVER_LIST = "REPLACE INTO approver_list (scripthash, publickey, status) VALUES (@scripthash, @publickey, @status)";
		private string SQL_INSERT_SMARTCONTRACT_MEMBER_LIST = "REPLACE INTO smartcontract_member_list (scripthash, publickey, status) VALUES (@scripthash, @publickey, @status)";
        private string SQL_INSERT_REGISTER_MULTISIGN_TRANSACTION = "REPLACE INTO register_multisign_transaction (txid, scripthash, address, pubkeys, time, block_number, hash) VALUES (@txid, @scripthash, @address, @pubkeys, @time, @block_number, @hash)";
        private string SQL_INSERT_REGISTER_MULTISIGN_HISTORY = "REPLACE INTO tx_history (txid, time, block_number, hash, tx_type, multisign_scripthash, multisign_address, multisign_pubkeys) VALUES (@txid, @time, @block_number, @hash, @tx_type, @multisign_scripthash, @multisign_address, @multisign_pubkeys)";

        private string SQL_GET_ASSET_NAME = "SELECT name FROM register_transaction WHERE txid = @txid";
        private string SQL_GET_ASSET_TYPE = "SELECT type FROM register_transaction WHERE txid = @txid";
        private string SQL_GET_HOLDER_ID = "SELECT id FROM holders WHERE address = @address AND asset = @asset";
        private string SQL_GET_TRANSACTIOM = "SELECT * FROM transactions WHERE txid = @txid";

		private string SQL_GET_STORAGE_SIZE = "SELECT current_size FROM storage_wallets WHERE address LIKE @storageaddr";
        private string SQL_GET_STATE_COUNT = "SELECT COUNT(*) FROM state_transaction WHERE from_address LIKE @fromaddr and state_script LIKE @statescript";
        private string SQL_GET_STORAGE_RATE = "SELECT rate FROM storage_wallets WHERE address LIKE @address";
        private string SQL_GET_STATE_TIME = "SELECT time FROM state_transaction WHERE txid LIKE @txid";

        private string SQL_UPDATE_CLAIM_UXTO = "UPDATE utxos SET claimed = 1 WHERE txid = @txid AND tx_out_index = @tx_out_index";

		private string SQL_UPDATE_STORAGE_SIZE = "UPDATE storage_wallets SET current_size = @currentsize WHERE address LIKE @storageaddr";
		private string SQL_UPDATE_UTXOS = "UPDATE utxos SET status = @status WHERE txid = @txid AND tx_out_index = @txoutindex";
        private string SQL_UPDATE_STORAGE_RATE = "UPDATE storage_wallets SET rate = @rate WHERE address LIKE @address";
        private string SQL_UPDATE_DB_STATUS = "UPDATE status SET current_block_number = @current_block_number, last_block_time = @last_block_time, block_version = @block_version";

        private MySqlConnection Connection = null;
        private MySqlTransaction MySqlTransaction = null;
        private MySqlCommand Command = null;

        public static DatabaseModel instance;

        public static DatabaseModel Default
        {
            get
            {
                if (instance == null)
                    instance = new DatabaseModel();

                return instance;
            }
        }

        public DatabaseModel()
        {
            string command = @"SERVER=" + Constants.Default.Database_IP + 
                ";PORT=" + Constants.Default.Database_PORT + 
                ";UID=" + Constants.Default.Database_USER + 
                ";PASSWORD=" + Constants.Default.Database_PASSWORD + 
                ";DATABASE=" + Constants.Default.Database_NAME;
            Connection = new MySqlConnection(command);

            instance = this;
        }

        public void TransactionStart()
        {
            Connection.Open();
            MySqlTransaction = Connection.BeginTransaction();
            Command = Connection.CreateCommand();
            Command.Transaction = MySqlTransaction;
            Command.Parameters.Clear();
        }

        public void Commit()
        {
            MySqlTransaction.Commit();
            MySqlTransaction = null;
            Command = null;
            Connection.Close();
        }

        public void RollBack()
        {
            if (MySqlTransaction != null)
            {
                MySqlTransaction.Rollback();

                MySqlTransaction = null;
                Command = null;
                Connection.Close();
            }
        }

        public int GetDatabaseBlockNumber()
        {
            int blockNumber = -1;
            try
            {
                string sql = "SELECT current_block_number FROM status";
                MySqlCommand cmd = new MySqlCommand(sql, Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    blockNumber = rdr.GetInt32(0);
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }
            return blockNumber;
        }

        public bool RegBlockDataOnDB(Block block)
        {
            try
            {
                Command.CommandText = SQL_INSERT_BLOCK;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@size", block.Size);
                Command.Parameters.AddWithValue("@version", block.Version);
                Command.Parameters.AddWithValue("@prevhash", block.PrevBlockHash);
                Command.Parameters.AddWithValue("@merkleroot", block.MerkleRoot);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@nonce", block.Nonce);
                Command.Parameters.AddWithValue("@currentconsensus", block.CurrentConsensus);
                Command.Parameters.AddWithValue("@nextconsensus", block.NextConsensus);
                Command.Parameters.AddWithValue("@script", block.Script);
                Command.Parameters.AddWithValue("@txcount", block.TxCount);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegStateTransactionDataOnDB(Block block, StateTransaction tx)
        {
            try
            {
                //@txid, @fromaddr, @toaddr, @uploadhash, @statescript, @time, @blocknum, @hash
                Command.CommandText = SQL_INSERT_STATE_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@fromaddr", tx.FromAddr);
                Command.Parameters.AddWithValue("@toaddr", tx.ToAddr);
                Command.Parameters.AddWithValue("@uploadhash", tx.TxHash);
                Command.Parameters.AddWithValue("@statescript", tx.StrStateScript);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //@txid, @time, @blocknum, @hash, @txtype, @fromaddr, @toaddr, @uploadhash, @statescript
                Command.CommandText = SQL_INSERT_STATE_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@fromaddr", tx.FromAddr);
                Command.Parameters.AddWithValue("@toaddr", tx.ToAddr);
                Command.Parameters.AddWithValue("@uploadhash", tx.TxHash);
                Command.Parameters.AddWithValue("@statescript", tx.StrStateScript);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);  
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegPayFileTransactionDataOnDB(Block block, PayFileTransaction tx)
        {
            try
            {
                //@txid, @downaddr, @upaddr, @asset, @name, @amount, @fee, @time, @blocknum, @hash, @dtxhash
                Command.CommandText = SQL_INSERT_PAYFILE_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@downaddr", tx.FromAddress);
                Command.Parameters.AddWithValue("@upaddr", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@asset", tx.Vout[0].Asset);
                Command.Parameters.AddWithValue("@name", tx.AssetName);
                Command.Parameters.AddWithValue("@amount", tx.Amount);
                Command.Parameters.AddWithValue("@fee", tx.Fee);
                Command.Parameters.AddWithValue("@dtxhash", tx.DtxHash);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //@txid, @time, @blocknum, @hash, @txtype, @downaddr, @upaddr, @asset, @name, @amount, @fee, @dtxhash
                Command.CommandText = SQL_INSERT_PAYFILE_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@downaddr", tx.FromAddress);
                Command.Parameters.AddWithValue("@upaddr", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@asset", tx.Vout[0].Asset);
                Command.Parameters.AddWithValue("@name", tx.AssetName);
                Command.Parameters.AddWithValue("@amount", tx.Amount);
                Command.Parameters.AddWithValue("@fee", tx.Fee);
                Command.Parameters.AddWithValue("@dtxhash", tx.DtxHash);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegApproveDownloadTransactionDataOnDB(Block block, ApproveDownloadTransaction tx)
        {
            try
            {
                //@txid, @approvaddr, @downaddr, @dtxhash, @approvstate, @time, @blocknum, @hash
                Command.CommandText = SQL_INSERT_APPROVEDOWNLOAD_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@approvaddr", tx.ApproveHash);
                Command.Parameters.AddWithValue("@downaddr", tx.DownloadHash);
                Command.Parameters.AddWithValue("@dtxhash", tx.DtxHash);
                Command.Parameters.AddWithValue("@approvstate", tx.ApproveState);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //@txid, @time, @blocknum, @hash, @txtype, @approvaddr, @downaddr, @dtxhash, @approvstate
                Command.CommandText = SQL_INSERT_APPROVEDOWNLOAD_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);
                Command.Parameters.AddWithValue("@approvaddr", tx.ApproveHash);
                Command.Parameters.AddWithValue("@downaddr", tx.DownloadHash);
                Command.Parameters.AddWithValue("@dtxhash", tx.DtxHash);
                Command.Parameters.AddWithValue("@approvstate", tx.ApproveState);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegDownloadRequestTransactionOnDB(Block block, DownloadRequestTransaction tx)
        {
            try
            {
                //@txid, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @downloadaddr, @filehash, @time, @blocknum, @hash
                Command.CommandText = SQL_INSERT_DOWNLOADREQUEST_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@filename", tx.FileName);
                Command.Parameters.AddWithValue("@filedescription", tx.FileDescription);
                Command.Parameters.AddWithValue("@fileurl", tx.FileUrl);
                Command.Parameters.AddWithValue("@amount", tx.PayAmount);
                Command.Parameters.AddWithValue("@uploadaddr", tx.UploadHash);
                Command.Parameters.AddWithValue("@downloadaddr", tx.DownloadHash);
                Command.Parameters.AddWithValue("@filehash", tx.UtxHash);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //@txid, @time, @blocknum, @hash, @txtype, @approvaddr, @downaddr, @dtxhash, @approvstate
                Command.CommandText = SQL_INSERT_DOWNLOADREQUEST_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);
                Command.Parameters.AddWithValue("@filename", tx.FileName);
                Command.Parameters.AddWithValue("@filedescription", tx.FileDescription);
                Command.Parameters.AddWithValue("@fileurl", tx.FileUrl);
                Command.Parameters.AddWithValue("@amount", tx.PayAmount);
                Command.Parameters.AddWithValue("@uploadaddr", tx.UploadHash);
                Command.Parameters.AddWithValue("@downloadaddr", tx.DownloadHash);
                Command.Parameters.AddWithValue("@filehash", tx.UtxHash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegPayUploadTransaction(Block block, PayUploadTransaction tx)
        {
            try
            {
                //@txid, @utxid, @uploadsize, @fromaddr, @toaddr, @asset, @name, @amount, @fee, @time, @blocknum, @hash
                Command.CommandText = SQL_INSERT_PAYUPLOAD_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@utxid", tx.UtxHash);
                Command.Parameters.AddWithValue("@uploadsize", tx.UploadByteSize);
                Command.Parameters.AddWithValue("@fromaddr", tx.FromAddress);
                Command.Parameters.AddWithValue("@toaddr", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@asset", tx.Vout[0].Asset);
                Command.Parameters.AddWithValue("@name", tx.AssetName);
                Command.Parameters.AddWithValue("@amount", tx.Amount);
                Command.Parameters.AddWithValue("@fee", tx.Fee);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //@txid, @time, @blocknum, @hash, @txtype, @approvaddr, @downaddr, @dtxhash, @approvstate
                Command.CommandText = SQL_INSERT_PAYUPLOAD_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);
                Command.Parameters.AddWithValue("@utxid", tx.UtxHash);
                Command.Parameters.AddWithValue("@uploadsize", tx.UploadByteSize);
                Command.Parameters.AddWithValue("@fromaddr", tx.FromAddress);
                Command.Parameters.AddWithValue("@toaddr", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@asset", tx.Vout[0].Asset);
                Command.Parameters.AddWithValue("@name", tx.AssetName);
                Command.Parameters.AddWithValue("@amount", tx.Amount);
                Command.Parameters.AddWithValue("@fee", tx.Fee);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool UpdateStorageSizeDataOnDB(PayUploadTransaction tx, Block block)
        {
            try
            {
                //@storageaddr
                Command.CommandText = SQL_GET_STORAGE_SIZE;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@storageaddr", tx.Vout[0].Address);

                MySqlDataReader rdr = Command.ExecuteReader();
                double currentSize = 0;

                if (rdr.Read())
                {
                    currentSize = rdr.GetDouble(0);
                    rdr.Close();
                }

                currentSize += tx.UploadByteSize;

                //@currentsize, @storageaddr
                Command.CommandText = SQL_UPDATE_STORAGE_SIZE;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@currentsize", currentSize);
                Command.Parameters.AddWithValue("@storageaddr", tx.Vout[0].Address);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegUploadRequestTransaction(Block block, UploadRequestTransaction tx)
        {
            try
            {
                //@txid, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @fileverifiers, @durationdays, @time, @blocknum, @hash
                Command.CommandText = SQL_INSERT_UPLOADREQUEST_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@filename", tx.FileName);
                Command.Parameters.AddWithValue("@filedescription", tx.FileDescription);
                Command.Parameters.AddWithValue("@fileurl", tx.FileUrl);
                Command.Parameters.AddWithValue("@amount", tx.PayAmount);
                Command.Parameters.AddWithValue("@uploadaddr", tx.UploadHash);
                Command.Parameters.AddWithValue("@fileverifiers", tx.FileVerifiers);
                Command.Parameters.AddWithValue("@durationdays", tx.DurationDays);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //(@txid, @time, @blocknum, @hash, @txtype, @filename, @filedescription, @fileurl, @amount, @uploadaddr, @fileverifiers, @durationdays)
                Command.CommandText = SQL_INSERT_UPLOADREQUEST_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);
                Command.Parameters.AddWithValue("@filename", tx.FileName);
                Command.Parameters.AddWithValue("@filedescription", tx.FileDescription);
                Command.Parameters.AddWithValue("@fileurl", tx.FileUrl);
                Command.Parameters.AddWithValue("@amount", tx.PayAmount);
                Command.Parameters.AddWithValue("@uploadaddr", tx.UploadHash);
                Command.Parameters.AddWithValue("@fileverifiers", tx.FileVerifiers);
                Command.Parameters.AddWithValue("@durationdays", tx.DurationDays);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegStorageWalletTransaction(Block block, StorageTransaction tx)
        {
            try
            {
                //@txid, @address, @storagesize, @guarantee, @payamount, @endtime
                Command.CommandText = SQL_INSERT_STORAGEWALLET_TRANSACTION;

                string address = CommonUtils.ToAddress(tx.OwnerHash);

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@address", address);
                Command.Parameters.AddWithValue("@storagesize", tx.StorageSize);
                Command.Parameters.AddWithValue("@guarantee", tx.GuaranteeAmount);
                Command.Parameters.AddWithValue("@payamount", tx.PayAmount);
                Command.Parameters.AddWithValue("@endtime", tx.EndtimeStamp);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                //(@txid, @time, @blocknum, @hash, @txtype, @address, @storagesize, @guarantee, @payamount, @endtime)
                Command.CommandText = SQL_INSERT_STORAGEWALLET_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@txtype", tx.Type);
                Command.Parameters.AddWithValue("@address", tx.OwnerHash);
                Command.Parameters.AddWithValue("@storagesize", tx.StorageSize);
                Command.Parameters.AddWithValue("@guarantee", tx.GuaranteeAmount);
                Command.Parameters.AddWithValue("@payamount", tx.PayAmount);
                Command.Parameters.AddWithValue("@endtime", tx.EndtimeStamp);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool UpdateUtxosStatusOnDB(Block block, Transaction tx)
        {
            try
            {
                foreach (TransactionInput input in tx.Vin)
                {
                    //@status, @txid, @txoutindex
                    Command.CommandText = SQL_UPDATE_UTXOS;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@status", Constants.Default.UTXOSTATUS_SPENT);
                    Command.Parameters.AddWithValue("@txid", input.Txid);
                    Command.Parameters.AddWithValue("@txoutindex", input.Vout);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool AddUtxosOnDB(Block block, Transaction tx)
        {
            try
            {
                foreach (TransactionOutput output in tx.Vout)
                {
                    //@txid, @txoutindex, @asset, @name, @value, @address, @status, @time, @blocknum, @hash
                    string AssetName = GetAssetName(output.Asset);

                    Command.CommandText = SQL_INSERT_UTXO;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@txid", tx.Txid);
                    Command.Parameters.AddWithValue("@txoutindex", output.Index);
                    Command.Parameters.AddWithValue("@asset", output.Asset);
                    Command.Parameters.AddWithValue("@name", AssetName);
                    Command.Parameters.AddWithValue("@value", output.Value);
                    Command.Parameters.AddWithValue("@address", output.Address);
                    Command.Parameters.AddWithValue("@status", Constants.Default.UTXOSTATUS_UNSPENT);
                    Command.Parameters.AddWithValue("@time", block.Time);
                    Command.Parameters.AddWithValue("@blocknum", block.BlockNumber);
                    Command.Parameters.AddWithValue("@hash", block.Hash);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool UpdatePermissionState(Block block, StateTransaction tx)
        {
            try
            {
                Console.WriteLine(tx.StateScript[0]);
                Console.WriteLine(tx.StateScript[1]);
                if ((STATE_TYPE)tx.StateScript[0] != STATE_TYPE.PERMISSION_STATES ||
                    tx.StateScript.Length < 3)
                    return true;
                    
                string publickey = "";

                int length = tx.StateScript[2];

                for (int i = 3; i < length + 3; i++)
                    publickey += (char)tx.StateScript[i];

                //@scripthash, @publickey, @status
                if (tx.StateScript[1] >= (byte)PERMISSION_STATES.PERMISSION_PRIVACY_SC_ON)
                    Command.CommandText = SQL_INSERT_SMARTCONTRACT_MEMBER_LIST;
                else
                    Command.CommandText = SQL_INSERT_APPROVER_LIST;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@scripthash", tx.FromAddr);
                Command.Parameters.AddWithValue("@publickey", publickey);
                Command.Parameters.AddWithValue("@status", tx.StateScript[1] % 2 + 1);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool UpdateStorageState(Block block, StateTransaction tx)
        {
            try
            {
                if ( tx.StateScript.Length < 2 || 
                     (STATE_TYPE)tx.StateScript[0] != STATE_TYPE.DOWNLOAD_STATES ||
                     (DOWNLOAD_STATES)tx.StateScript[1] != DOWNLOAD_STATES.DOWNLOAD_START_REPLY )
                    return true;

                int nFileCount = GetStateCount(tx.FromAddr, "0105"); // Get Downloaded file count
                int time = GetStateTime(tx.TxHash);
                double rate = GetStorageRate(CommonUtils.ToAddress(tx.FromAddr)); // !Address not scripthash!

                int fileSuccessCount = (int)((rate * (nFileCount - 1)) / 5);
                double newRate = 5;

                if (block.Time - time > 4000)
                    newRate = 5.0 * fileSuccessCount / nFileCount;
                else
                    newRate = 5.0 * (fileSuccessCount + 1) / nFileCount;

                //@rate, @address
                Command.CommandText = SQL_UPDATE_STORAGE_RATE;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@rate", newRate);
                Command.Parameters.AddWithValue("@address", tx.FromAddr);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }
        
        public bool RegTransactionDataOnDB(Block block, Transaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@size", tx.Size);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);
                Command.Parameters.AddWithValue("@version", tx.Version);
                Command.Parameters.AddWithValue("@attribute", tx.Attributes);
                Command.Parameters.AddWithValue("@vin", tx.JVin);
                Command.Parameters.AddWithValue("@vout", tx.JVout);
                Command.Parameters.AddWithValue("@sys_fee", tx.SysFee);
                Command.Parameters.AddWithValue("@net_fee", tx.NetFee);
                Command.Parameters.AddWithValue("@scripts", tx.Scripts);
                Command.Parameters.AddWithValue("@nonce", tx.Nonce);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegRegisterTransactionDataOnDB(Block block, RegisterTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_REGISTER_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@type", tx.Asset.Type);
                Command.Parameters.AddWithValue("@name", tx.Asset.Name);
                Command.Parameters.AddWithValue("@amount", tx.Asset.Amount);
                Command.Parameters.AddWithValue("@_precision", tx.Asset.Precision);
                Command.Parameters.AddWithValue("@owner", tx.Asset.Owner);
                Command.Parameters.AddWithValue("@admin", tx.Asset.Admin);
                Command.Parameters.AddWithValue("@tfee_min", tx.Asset.TFeeMin);
                Command.Parameters.AddWithValue("@tfee_max", tx.Asset.TFeeMax);
                Command.Parameters.AddWithValue("@afee", tx.Asset.AFee);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegIssueTransactionDataOnDB(Block block, IssueTransaction tx)
        {
            try
            {
                for (int i = 0; i < tx.Vout.Length; i++)
                {
                    if (block.BlockNumber != 0 && tx.Vout[i].Address == tx.FromAddress && tx.Vout[i].Asset != tx.Vout[0].Asset)
                    {
                        continue;
                    }

                    string assetName = DatabaseModel.Default.GetAssetName(tx.Vout[i].Asset);

                    Command.CommandText = SQL_INSERT_ISSUE_TRANSACTION;
                    Command.Parameters.Clear();

                    Command.Parameters.AddWithValue("@txid", tx.Txid);
                    Command.Parameters.AddWithValue("@type", tx.Type);
                    Command.Parameters.AddWithValue("@asset", tx.Vout[i].Asset);
                    Command.Parameters.AddWithValue("@name", assetName);
                    Command.Parameters.AddWithValue("@address", tx.FromAddress);
                    Command.Parameters.AddWithValue("@_to", tx.Vout[i].Address);
                    Command.Parameters.AddWithValue("@amount", tx.Vout[i].Value);
                    Command.Parameters.AddWithValue("@fee", tx.NetFee < 0? 0: tx.NetFee);
                    Command.Parameters.AddWithValue("@time", block.Time);
                    Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                    Command.Parameters.AddWithValue("@hash", block.Hash);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");


                    Command.CommandText = SQL_INSERT_ISSUE_HISTORY;
                    Command.Parameters.Clear();

                    Command.Parameters.AddWithValue("@txid", tx.Txid);
                    Command.Parameters.AddWithValue("@time", block.Time);
                    Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                    Command.Parameters.AddWithValue("@hash", block.Hash);
                    Command.Parameters.AddWithValue("@tx_type", tx.Type);
                    Command.Parameters.AddWithValue("@issue_transaction_address", tx.FromAddress);
                    Command.Parameters.AddWithValue("@issue_transaction_asset", tx.Vout[i].Asset);
                    Command.Parameters.AddWithValue("@issue_transaction_to", tx.Vout[i].Address);
                    Command.Parameters.AddWithValue("@issue_transaction_amount", tx.Vout[i].Value);
                    Command.Parameters.AddWithValue("@issue_transaction_fee", tx.NetFee < 0? 0: tx.NetFee);
                    Command.Parameters.AddWithValue("@name", tx.AssetName);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegClaimTransactionDataOnDB(Block block, ClaimTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_CLAIM_TRANSACTION;
                Command.Parameters.Clear();

                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@claims", tx.JClaims);
                Command.Parameters.AddWithValue("@address", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@amount", tx.Vout[0].Value);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_CLAIM_HISTORY;
                Command.Parameters.Clear();

                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);
                Command.Parameters.AddWithValue("@claim_transaction_address", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@claim_transaction_amount", tx.Vout[0].Value);
                Command.Parameters.AddWithValue("@name", "XQG");

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool ClaimTransactionToUtxosStatusOnDB(Block block, ClaimTransaction tx)
        {
            try
            {
                for (int i = 0; i < tx.Claims.Length; i++)
                {
                    Command.CommandText = SQL_UPDATE_CLAIM_UXTO;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@txid", tx.Claims[i].Txid);
                    Command.Parameters.AddWithValue("@tx_out_index", tx.Claims[i].Vout);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool EnrollmentTransactionOnDB(Block block, EnrollmentTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_ENROLLMENT_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@pubkey", tx.Pubkey);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool ContractTransactionOnDB(Block block, ContractTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_CONTRACT_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@_from", tx.FromAddress);
                Command.Parameters.AddWithValue("@_to", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@asset", tx.Vout[0].Asset);
                Command.Parameters.AddWithValue("@name", tx.AssetName);
                Command.Parameters.AddWithValue("@amount", tx.Amount);
                Command.Parameters.AddWithValue("@fee", tx.Fee);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_CONTRACT_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);
                Command.Parameters.AddWithValue("@contract_transaction_from", tx.FromAddress);
                Command.Parameters.AddWithValue("@contract_transaction_to", tx.Vout[0].Address);
                Command.Parameters.AddWithValue("@contract_transaction_asset", tx.Vout[0].Asset);
                Command.Parameters.AddWithValue("@contract_transaction_amount", tx.Amount);
                Command.Parameters.AddWithValue("@contract_transaction_fee", tx.Fee);
                Command.Parameters.AddWithValue("@name", tx.AssetName);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool IssueHoldersOnDB(Block block, IssueTransaction tx)
        {
            try
            {
                if (GetAssetType(tx.Vout[0].Asset) != "TransparentToken")
                {
                    return true;
                }

                string asset = tx.Vout[0].Asset;

                for (int i = 0; i < tx.Vout.Length; i++)
                {
                    if (tx.Vout[i].Address == tx.FromAddress && tx.Vout[i].Asset != asset)
                    {
                        continue;
                    }

                    if (IsExistHolder(tx.Vout[i].Address, asset))
                    {
                        Command.CommandText = SQL_UPDATE_TO_HOLDER;

                        Command.Parameters.Clear();
                        Command.Parameters.AddWithValue("@amount", tx.Vout[i].Value);
                        Command.Parameters.AddWithValue("@address", tx.Vout[i].Address);
                        Command.Parameters.AddWithValue("@asset", asset);

                        if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                    }
                    else
                    {
                        Command.CommandText = SQL_INSERT_HOLDERS;

                        Command.Parameters.Clear();
                        Command.Parameters.AddWithValue("@address", tx.Vout[i].Address);
                        Command.Parameters.AddWithValue("@name", tx.AssetName);
                        Command.Parameters.AddWithValue("@asset", asset);
                        Command.Parameters.AddWithValue("@balance", tx.Vout[i].Value);

                        if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                    }                    
                }
                
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool ContractHoldersOnDB(Block block, ContractTransaction tx)
        {
            try
            {
                if (GetAssetType(tx.Vout[0].Asset) != "TransparentToken")
                {
                    return true;
                }

                string asset = tx.Vout[0].Asset;

                Command.CommandText = SQL_UPDATE_FROM_HOLDER;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@amount", tx.Amount);
                Command.Parameters.AddWithValue("@address", tx.FromAddress);
                Command.Parameters.AddWithValue("@asset", asset);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                if (IsExistHolder(tx.Vout[0].Address, asset))
                {
                    Command.CommandText = SQL_UPDATE_TO_HOLDER;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@amount", tx.Amount);
                    Command.Parameters.AddWithValue("@address", tx.Vout[0].Address);
                    Command.Parameters.AddWithValue("@asset", asset);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }
                else
                {
                    Command.CommandText = SQL_INSERT_HOLDERS;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@address", tx.Vout[0].Address);
                    Command.Parameters.AddWithValue("@name", tx.AssetName);
                    Command.Parameters.AddWithValue("@asset", asset);
                    Command.Parameters.AddWithValue("@balance", tx.Vout[0].Value);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool AnonymousContractTransactionOnDB(Block block, AnonymousContractTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_ANONYMOUS_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_ANONYMOUS_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RingConfidentTransactionOnDB(Block block, RingConfidentialTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_RING_CONFIDENT_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_RING_CONFIDENT_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool PublishTransactionOnDB(Block block, PublishTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_PUBLISH_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@contract_code_hash", tx.Code.Hash);
                Command.Parameters.AddWithValue("@contract_code_script", tx.Code.Script);
                Command.Parameters.AddWithValue("@contract_code_parameters", tx.Code.Parameters);
                Command.Parameters.AddWithValue("@contract_code_returntype", tx.Code.Returntype);
                Command.Parameters.AddWithValue("@contract_needstorage", tx.NeedStorage);
                Command.Parameters.AddWithValue("@contract_name", tx.Name);
                Command.Parameters.AddWithValue("@contract_version", tx.Version);
                Command.Parameters.AddWithValue("@contract_author", tx.Author);
                Command.Parameters.AddWithValue("@contract_email", tx.Email);
                Command.Parameters.AddWithValue("@contract_description", tx.Description);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool InvocationTransactionOnDB(Block block, InvocationTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_INVOCATION_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@address", tx.FromAddress);
                Command.Parameters.AddWithValue("@script", tx.Script);
                Command.Parameters.AddWithValue("@gas", tx.Gas);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_INVOCATION_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);
                Command.Parameters.AddWithValue("@invocation_transaction_address", tx.FromAddress);
                Command.Parameters.AddWithValue("@gas", tx.Gas);
                Command.Parameters.AddWithValue("@name", "XQG");

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegAssetTransactionOnDB(Block block, InvocationTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_REGISTER_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@type", tx.Asset.Type);
                Command.Parameters.AddWithValue("@name", tx.Asset.Name);
                Command.Parameters.AddWithValue("@amount", tx.Asset.Amount);
                Command.Parameters.AddWithValue("@_precision", tx.Asset.Precision);
                Command.Parameters.AddWithValue("@owner", tx.Asset.Owner);
                Command.Parameters.AddWithValue("@admin", tx.Asset.Admin);
                Command.Parameters.AddWithValue("@tfee_min", tx.Asset.TFeeMin);
                Command.Parameters.AddWithValue("@tfee_max", tx.Asset.TFeeMax);
                Command.Parameters.AddWithValue("@afee", tx.Asset.AFee);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool MinerTransactionOnDB(Block block, MinerTransaction tx)
        {
            try
            {
                double amount = 0;

                for (int i = 0; i < tx.Vout.Length; i++)
                {
                    if (tx.Vout[i].Address == block.CurrentConsensus)
                    {
                        amount = amount + tx.Vout[i].Value;
                        continue;
                    }

                    Command.CommandText = SQL_INSERT_MINER_TRANSACTION;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@txid", tx.Txid);
                    Command.Parameters.AddWithValue("@address", block.CurrentConsensus);
                    Command.Parameters.AddWithValue("@_to", tx.Vout[i].Address);
                    Command.Parameters.AddWithValue("@amount", tx.Vout[i].Value);
                    Command.Parameters.AddWithValue("@nonce", tx.Nonce);
                    Command.Parameters.AddWithValue("@time", block.Time);
                    Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                    Command.Parameters.AddWithValue("@hash", block.Hash);

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                    Command.CommandText = SQL_INSERT_MINER_HISTORY;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@txid", tx.Txid);
                    Command.Parameters.AddWithValue("@time", block.Time);
                    Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                    Command.Parameters.AddWithValue("@hash", block.Hash);
                    Command.Parameters.AddWithValue("@tx_type", tx.Type);
                    Command.Parameters.AddWithValue("@miner_transaction_address", block.CurrentConsensus);
                    Command.Parameters.AddWithValue("@miner_transaction_to", tx.Vout[i].Address);
                    Command.Parameters.AddWithValue("@miner_transaction_amount", tx.Vout[i].Value);
                    Command.Parameters.AddWithValue("@name", "XQG");

                    if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                }

                Command.CommandText = SQL_INSERT_MINER_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@address", block.CurrentConsensus);
                Command.Parameters.AddWithValue("@_to", block.CurrentConsensus);
                Command.Parameters.AddWithValue("@amount", amount);
                Command.Parameters.AddWithValue("@nonce", tx.Nonce);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_MINER_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);
                Command.Parameters.AddWithValue("@miner_transaction_address", block.CurrentConsensus);
                Command.Parameters.AddWithValue("@miner_transaction_to", block.CurrentConsensus);
                Command.Parameters.AddWithValue("@miner_transaction_amount", amount);
                Command.Parameters.AddWithValue("@name", "XQG");

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool RegisterMultiSignTransactionOnDB(Block block, RegisterMultiSignTransaction tx)
        {
            try
            {
                Command.CommandText = SQL_INSERT_REGISTER_MULTISIGN_TRANSACTION;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@scripthash", tx.MultiSigAddressScript);
                Command.Parameters.AddWithValue("@address", tx.MultiSigAddress);
                Command.Parameters.AddWithValue("@pubkeys", tx.JAddresses);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                Command.CommandText = SQL_INSERT_REGISTER_MULTISIGN_HISTORY;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", tx.Txid);
                Command.Parameters.AddWithValue("@time", block.Time);
                Command.Parameters.AddWithValue("@block_number", block.BlockNumber);
                Command.Parameters.AddWithValue("@hash", block.Hash);
                Command.Parameters.AddWithValue("@tx_type", tx.Type);
                Command.Parameters.AddWithValue("@multisign_scripthash", tx.MultiSigAddressScript);
                Command.Parameters.AddWithValue("@multisign_address", tx.MultiSigAddress);
                Command.Parameters.AddWithValue("@multisign_pubkeys", tx.JAddresses);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public bool UpdateTaskId(Block block, int current_block_num)
        {
            try
            {
                // @current_block_number, @last_block_time, @block_version
                Command.CommandText = SQL_UPDATE_DB_STATUS;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@current_block_number", current_block_num + 1);
                Command.Parameters.AddWithValue("@last_block_time", block.Time);
                Command.Parameters.AddWithValue("@block_version", block.Version);

                if (Command.ExecuteNonQuery() == -1) throw new Exception("SQL execution failed.");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
                return false;
            }
        }

        public string GetFromAddress(Transaction tx)
        {
            string retAddress = "";

            try
            {
                if (tx.Vin != null && tx.Vin.Length > 0)
                {
                    string txID = tx.Vin[0].Txid;
                    int vout = tx.Vin[0].Vout;
                    string txVOut = "";

                    Command.CommandText = SQL_GET_TRANSACTIOM;

                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@txid", txID);

                    MySqlDataReader rdr = Command.ExecuteReader();

                    if (rdr.Read())
                    {
                        txVOut = rdr.GetString(6);
                    }

                    if (rdr != null && rdr.IsClosed == false)
                    {
                        rdr.Close();
                    }

                    JArray jVOut = JArray.Parse(txVOut);
                    JObject jOutItem = JObject.Parse(jVOut[vout].ToString());

                    retAddress = jOutItem.GetValue("address").ToString();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }
            return retAddress;
        }

        public string GetAssetName(string assetID)
        {
            String retAssetName = "";

            try
            {
                Command.CommandText = SQL_GET_ASSET_NAME;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", assetID);

                MySqlDataReader rdr = Command.ExecuteReader();

                if (rdr.Read())
                {
                    retAssetName = rdr.GetString(0);
                }

                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return retAssetName;
        }

        public string GetAssetType(string assetID)
        {
            String retAssetType = "";

            try
            {
                Command.CommandText = SQL_GET_ASSET_TYPE;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", assetID);

                MySqlDataReader rdr = Command.ExecuteReader();

                if (rdr.Read())
                {
                    retAssetType = rdr.GetString(0);
                }

                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return retAssetType;
        }

        public bool IsExistHolder(string address, string asset)
        {
            bool retIsExist = false;

            try
            {
                Command.CommandText = SQL_GET_HOLDER_ID;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@address", address);
                Command.Parameters.AddWithValue("@asset", asset);

                MySqlDataReader rdr = Command.ExecuteReader();

                if (rdr.Read())
                {
                    retIsExist = true;
                }

                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return retIsExist;
        }

        public int GetStateCount(string scripthash, string statescript)
        {
            int retStateCount = 0;

            try
            {
                Command.CommandText = SQL_GET_STATE_COUNT;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@fromaddr", scripthash);
                Command.Parameters.AddWithValue("@statescript", statescript);

                MySqlDataReader rdr = Command.ExecuteReader();

                if (rdr.Read())
                {
                    retStateCount = rdr.GetInt32(0);
                }

                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return retStateCount;
        }

        public double GetStorageRate(string address)
        {
            double retStorageRate = 0;

            try
            {
                Command.CommandText = SQL_GET_STORAGE_RATE;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@address", address);

                MySqlDataReader rdr = Command.ExecuteReader();

                if (rdr.Read())
                {
                    retStorageRate = rdr.GetDouble(0);
                }

                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return retStorageRate;
        }

        public int GetStateTime(string txid)
        {
            int retStateTime = 0;

            try
            {
                Command.CommandText = SQL_GET_STATE_TIME;

                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@txid", txid);

                MySqlDataReader rdr = Command.ExecuteReader();

                if (rdr.Read())
                {
                    retStateTime = rdr.GetInt32(0);
                }

                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtils.Default.Log(ex.Message + '\n' + ex.StackTrace);
            }

            return retStateTime;
        }
    }
}
