using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SoftResWA.Util
{
    public static class Protector
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ShifuiK1"); // 8 caracteres
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("KaySopA2");   // 8 caracteres

        // Cifra un texto plano
        public static string Cifrar(string textoPlano)
        {
            using (var des = new DESCryptoServiceProvider())
            using (var memStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
            {
                byte[] data = Encoding.UTF8.GetBytes(textoPlano);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memStream.ToArray())
                    .Replace('+', '-').Replace('/', '_').Replace("=", "");
            }
        }

        // Descifra un texto cifrado
        public static string Descifrar(string textoCifrado)
        {
            try
            {
                textoCifrado = textoCifrado.Replace('-', '+').Replace('_', '/');
                int padding = (4 - textoCifrado.Length % 4) % 4;
                textoCifrado = textoCifrado.PadRight(textoCifrado.Length + padding, '=');

                byte[] data = Convert.FromBase64String(textoCifrado);
                using (var des = new DESCryptoServiceProvider())
                using (var memStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memStream, des.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(memStream.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }
    }
}