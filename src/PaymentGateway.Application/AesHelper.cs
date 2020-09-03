using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PaymentGateway.Application.Abstractions;

namespace PaymentGateway.Application
{
    public class AesHelper : IAesHelper
    {
        private readonly string _key;
        private byte[] _iv;

        public AesHelper(string key)
        {
            _key = key;
            _iv = new byte[16];
        }

        public string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentNullException(nameof(_key));
            }

            var keyBytes = Encoding.UTF8.GetBytes(_key);

            using var aesAlg = Aes.Create();
            var encryptor = aesAlg.CreateEncryptor(keyBytes, _iv);
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(value);
            }

            var encrypted = msEncrypt.ToArray();

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentNullException(nameof(_key));
            }

            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            using Aes aesAlg = Aes.Create();
            var decryptor = aesAlg.CreateDecryptor(keyBytes, _iv);

            using var msDecrypt = new MemoryStream(cipherTextBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            var plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }
    }
}