using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encrypt
{
    class Encryption
    {
        private string masterPassword;
        private byte[] initializationVector;
        private byte[] masterPasswordByte;
        public Encryption(String masterPassword) {
            this.masterPassword = masterPassword;
            this.initializationVector = System.Text.Encoding.UTF8.GetBytes("12345678");
            this.masterPasswordByte = System.Text.Encoding.UTF8.GetBytes(hashTo16Bits(masterPassword));
            //this.masterPasswordByte = System.Text.Encoding.UTF8.GetBytes("87654321");
        }
        public String EncryptPassword(String password) {
            String ReturnString = "";
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] passwordByte = System.Text.Encoding.UTF8.GetBytes(password);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
                des.Padding = PaddingMode.Zeros;
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(masterPasswordByte, initializationVector), CryptoStreamMode.Write);
                cs.Write(passwordByte, 0, passwordByte.Length);
                cs.FlushFinalBlock();
                ReturnString = Convert.ToBase64String(ms.ToArray());
            }
            return ReturnString;
        }

        public String DecryptPassword(String EncryptedPassword) {
            String ReturnString = "";
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] encryptedPasswordByte = new byte[EncryptedPassword.Replace(" ", "+").Length];
            encryptedPasswordByte = Convert.FromBase64String(EncryptedPassword.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
                des.Padding = PaddingMode.Zeros;
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(masterPasswordByte, initializationVector), CryptoStreamMode.Write);
                cs.Write(encryptedPasswordByte, 0, encryptedPasswordByte.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                ReturnString = encoding.GetString(ms.ToArray());
            }
            return ReturnString;
        }

        public String hashTo16Bits(String masterPassword) {
            byte[] bytes = Encoding.UTF8.GetBytes(masterPassword);

            SHA256Managed hashString = new SHA256Managed();
            byte[] hash = hashString.ComputeHash(bytes);

            char[] returnHash = new char[8];
            for (int i = 0; i < returnHash.Length; i++)
            {
                returnHash[i] = masterPassword[hash[i] % masterPassword.Length];
            }

            return new string(returnHash);
        }
    }
}