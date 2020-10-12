using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quras
{
    public static class CommonUtils
    {
        private static ThreadLocal<SHA256> _sha256 = new ThreadLocal<SHA256>(() => SHA256.Create());

        public static byte[] HexStringToByte(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }

        public static byte[] Sha256(this IEnumerable<byte> value)
        {
            return _sha256.Value.ComputeHash(value.ToArray());
        }

        public static string ToAddress(string scripthash)
        {
            if (scripthash.Substring(0, 2) == "0x")
                scripthash = scripthash.Substring(2);
            byte[] data = new byte[21];
            data[0] = Constants.Default.AddressVersion;
            byte[] byte_data = HexStringToByte(scripthash);
            Buffer.BlockCopy(byte_data.Reverse().ToArray(), 0, data, 1, 20);
            return Base58CheckEncode(data);
        }

        public static string Base58CheckEncode(byte[] data)
        {
            byte[] checksum = data.Sha256().Sha256();
            byte[] buffer = new byte[data.Length + 4];
            Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
            Buffer.BlockCopy(checksum, 0, buffer, data.Length, 4);
            return Base58.Encode(buffer);
        }
    }
}
