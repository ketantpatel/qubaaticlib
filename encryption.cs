using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Encryption
{
    public class Encryptor
    {
        public static byte[] EncryptionKey = { 91, 9, 171, 235, 29, 69, 96, 114, 208, 2, 203, 151, 45, 43, 107, 66, 101, 42, 20, 31, 20, 63, 213, 228, 3, 158, 125, 229, 200, 1, 119, 204 };
        public static byte[] EncryptionIV = { 67, 102, 40, 201, 248, 114, 169, 244, 150, 71, 190, 243, 5, 12, 103, 218 };

        public static byte[] GenerateKey()
        {
            RijndaelManaged provider = new RijndaelManaged();
            provider.GenerateKey();
            return provider.Key;
        }

        public static byte[] GenerateIV()
        {
            RijndaelManaged provider = new RijndaelManaged();
            provider.GenerateIV();
            return provider.IV;
        }

        public static string Decrypt(string encodedText, byte[] key, byte[] iv)
        {
            RijndaelManaged provider = new RijndaelManaged();
            encodedText = encodedText.Replace(" ", "+");
            byte[] encryptedBytes = Convert.FromBase64String(encodedText);
            MemoryStream ms = new MemoryStream();
            try
            {
                using (CryptoStream cryptoStream = new CryptoStream(ms, provider.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
            }
            catch { }
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string Encrypt(string value, byte[] key, byte[] iv)
        {
            RijndaelManaged provider = new RijndaelManaged();
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            MemoryStream ms = new MemoryStream();

            using (CryptoStream cryptoStream = new CryptoStream(ms, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cryptoStream.Write(valueBytes, 0, valueBytes.Length);
            }

            return Convert.ToBase64String(ms.ToArray());

        }
        public static string Decrypt(string toDecrypt, string key, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string Encrypt(string toEncrypt, string key, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}