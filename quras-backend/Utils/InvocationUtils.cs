using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class InvocationUtils
    {
        public static string GetUINT160(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte)ms.ReadByte();
                if (OpCode.PUSHBYTES20 != opcode)
                {
                    return "";
                }

                byte[] byRet = new byte[opcode + 1];
                byRet[0] = 0x1F;        //Transparent Address Version
                ms.Read(byRet, 1, opcode);

                return CommonUtils.Base58CheckEncode(byRet);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static double GetFixed8(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte)ms.ReadByte();
                if (OpCode.PUSHBYTES8 != opcode)
                {
                    return -1;
                }

                byte[] byRet = new byte[opcode];
                ms.Read(byRet, 0, opcode);

                //Array.Reverse(byRet);

                return BitConverter.ToInt64(byRet, 0) / 100000000d;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int GetByte(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte)ms.ReadByte();

                if (opcode == OpCode.PUSH0) return 0;
                if (opcode == OpCode.PUSHM1) return 1;
                if (opcode >= OpCode.PUSH1 && opcode <= OpCode.PUSH16) return GetNumberFromOPCode(opcode);

                if (opcode != 0x01) return -1;
                int ret = ms.ReadByte();
                return ret;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static int GetNumberFromOPCode(byte opcode)
        {
            int ret = opcode - OpCode.PUSH1 + 1;
            if (ret >= 0 && ret <= 16) return ret;
            return -1;
        }

        public static string GetString(MemoryStream ms)
        {
            try
            {
                byte[] byRet = null;
                int length = 0;
                byte opcode = (byte) ms.ReadByte();
                
                if (OpCode.PUSHBYTES75 >= opcode)
                {
                    length = opcode;
                    byRet = new byte[length];
                }
                else if (OpCode.PUSHDATA1 == opcode)
                {
                    length = ms.ReadByte();
                    byRet = new byte[length];
                }
                else if (OpCode.PUSHDATA2 == opcode)
                {
                    byte[] byLength = new byte[2];
                    ms.Read(byLength, 0, 2);
                    length = BitConverter.ToInt16(byLength, 0);
                    byRet = new byte[length];
                }
                else if (OpCode.PUSHDATA4 == opcode)
                {
                    byte[] byLength = new byte[4];
                    ms.Read(byLength, 0, 4);
                    length = BitConverter.ToInt16(byLength, 0);
                    byRet = new byte[length];
                }
                else
                {
                    return "";
                }

                if (length <= 0)
                {
                    return "";
                }

                ms.Read(byRet, 0, length);

                return Encoding.UTF8.GetString(byRet);
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static string GetSysCall(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte) ms.ReadByte();
                if (OpCode.SYSCALL != opcode)
                {
                    return "";
                }

                int length = ms.ReadByte();
                byte[] byRet = new byte[length];
                ms.Read(byRet, 0, length);

                return Encoding.ASCII.GetString(byRet);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool SkipECPoint(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte) ms.ReadByte();

                byte[] byRet = new byte[opcode];
                ms.Read(byRet, 0, opcode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetAssetType(int assetType)
        {
            switch(assetType)
            {
                case (int) AssetType.CreditFlag:
                    return "CreditFlag";
                case (int)AssetType.DutyFlag:
                    return "DutyFlag";
                case (int)AssetType.GoverningToken:
                    return "GoverningToken";
                case (int)AssetType.UtilityToken:
                    return "UtilityToken";
                case (int)AssetType.Currency:
                    return "Currency";
                case (int)AssetType.Share:
                    return "Share";
                case (int)AssetType.Invoice:
                    return "Invoice";
                case (int)AssetType.Token:
                    return "Token";
                case (int)AssetType.AnonymousToken:
                    return "AnonymousToken";
                case (int)AssetType.TransparentToken:
                    return "TransparentToken";
                case (int)AssetType.StealthToken:
                    return "StealthToken";
                default:
                    return "";
            }
        }
    }
}
