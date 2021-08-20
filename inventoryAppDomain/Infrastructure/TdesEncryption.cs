using System;
using System.Security.Cryptography;
using System.Text;

namespace inventoryAppDomain.Infrastructure
{
    public static class TdesEncryption
    {
        public static string GetEncryptionKey(string secretKey)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] hashedSecret = md5.ComputeHash(secretKeyBytes, 0, secretKeyBytes.Length);
            byte[] hashedSecretLast12Bytes = new byte[12];
            Array.Copy(hashedSecret, hashedSecret.Length - 12, hashedSecretLast12Bytes, 0, 12);
            String hashedSecretLast12HexString = BitConverter.ToString(hashedSecretLast12Bytes);
            hashedSecretLast12HexString = hashedSecretLast12HexString.ToLower().Replace("-", "");
            String secretKeyFirst12 = secretKey.Replace("FLWSECK-", "").Substring(0, 12);
            byte[] hashedSecretLast12HexBytes = Encoding.UTF8.GetBytes(hashedSecretLast12HexString);
            byte[] secretFirst12Bytes = Encoding.UTF8.GetBytes(secretKeyFirst12);
            byte[] combineKey = new byte[24];
            Array.Copy(secretFirst12Bytes, 0, combineKey, 0, secretFirst12Bytes.Length);
            Array.Copy(hashedSecretLast12HexBytes, hashedSecretLast12HexBytes.Length - 12, combineKey, 12, 12);
            return Encoding.UTF8.GetString(combineKey);
        }

        public static string Encrypt(string data, string encryptionKey)
        {
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            des.Key = Encoding.UTF8.GetBytes(encryptionKey);
            ICryptoTransform cryptoTransform = des.CreateEncryptor();
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedDataBytes = cryptoTransform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            des.Clear();
            return Convert.ToBase64String(encryptedDataBytes);
        }

        public static string Decrypt(string encryptedData, string encryptionKey)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(encryptionKey), Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryptoTransform = des.CreateDecryptor();
            byte[] encryptDataBytes = Convert.FromBase64String(encryptedData);
            byte[] plainDataBytes = cryptoTransform.TransformFinalBlock(encryptDataBytes, 0, encryptDataBytes.Length);
            des.Clear();
            return Encoding.UTF8.GetString(plainDataBytes);
        }
    }
}