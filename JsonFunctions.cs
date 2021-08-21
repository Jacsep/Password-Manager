using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

using Encrypt;
using PasswordFunctions;

namespace JsonFunctions
{
    class jsonModifiers
    {
        private String filename;
        private List<ManagerFunctions.Password> PasswordList;
        public jsonModifiers(String filename) {
            this.filename = filename;
            this.PasswordList = new List<ManagerFunctions.Password>();
        }
        
        public void loadJsonFile()
        {
            var PasswordList = new List<ManagerFunctions.Password>();
            if (File.Exists("EncryptedPasswords.json") && new FileInfo("EncryptedPasswords.json").Length > 0)
            {
                var JSONData = System.IO.File.ReadAllText("EncryptedPasswords.json");
                this.PasswordList = JsonSerializer.Deserialize<List<ManagerFunctions.Password>>(JSONData);
            }
        }

        public bool findCorrespondingPassword(String program, Encryption encryption) {
            foreach (ManagerFunctions.Password pass in PasswordList) {
                if (pass.Program.Equals(program)) {
                    Console.WriteLine("Password: " + encryption.DecryptPassword(pass.EncryptedPassword));
                    return true;
                }
            }
            return false;
        }

        public bool changePassword(String program, String password) {
            foreach (ManagerFunctions.Password pass in PasswordList) {
                if (pass.Program.Equals(program)) {
                    pass.EncryptedPassword = password;
                    return true;
                }
            }
            return false;
        }

        public bool addPassword(String program, String password) {
            foreach (ManagerFunctions.Password pass in PasswordList) {
                if (pass.Program.Equals(program)) {
                    return false;
                }
            }

            var newPassword = new ManagerFunctions.Password {
                Program = program,
                EncryptedPassword = password
            };

            PasswordList.Add(newPassword);
            return true;
        }

        public bool removePassword(String program) {
            foreach (ManagerFunctions.Password pass in PasswordList) {
                if (pass.Program.Equals(program)) {
                    PasswordList.Remove(pass);
                    return true;
                }
            }
            return false;
        }

        public void logOut() {
            var JSONData = JsonSerializer.Serialize<List<ManagerFunctions.Password>>(PasswordList);
            File.WriteAllText("EncryptedPasswords.json", JSONData);
        }
    }
}