using System;
using Encrypt;
using JsonFunctions;

namespace PasswordFunctions
{
    public class ManagerFunctions
    {
        private jsonModifiers savedPasswords;
        private Encryption encryption;
        public String masterPassword;

        public void startUpAndRun() {
            Console.WriteLine("================================");
            Console.WriteLine("Welcome To Your Password Manager");
            Console.WriteLine("================================");
            Console.Write("Please Enter Master Password: ");

            masterPassword = hideAndEnterPassword();
            encryption = new Encryption(masterPassword);

            savedPasswords = new jsonModifiers("EncryptedPasswords.json");
            savedPasswords.loadJsonFile();

            Console.WriteLine("What would you like to do?");
            
            runCommands();
        }

        public void runCommands() {
            while (true) {
                Console.WriteLine("============================================================================================");
                Console.WriteLine("(G) Get Password  (A) Add New Password  (R) Remove Password  (C) Change Password  (L) LogOut");
                Console.WriteLine("============================================================================================");
                Console.Write("Enter Command: ");
                String command = Console.ReadLine();
                if (command.Equals("L")) {
                    Console.Write("Saving and exiting password manager");
                    savedPasswords.logOut();
                    break;
                } else if (command.Equals("G")) {
                    Console.Write("Please enter the program you wish to get the password for: ");
                    String program = Console.ReadLine();
                    getPassword(program);
                } else if (command.Equals("A")) {
                    Console.Write("Please enter the program: ");
                    String program = Console.ReadLine();
                    Console.Write("Please enter the desired password: ");
                    String password = hideAndEnterPassword();
                    addPassword(program, password);
                } else if (command.Equals("R")) {
                    Console.Write("Please enter the program you wish to delete the password for: ");
                    String program = Console.ReadLine();
                    removePassword(program);
                } else if (command.Equals("C")) {
                    Console.Write("Please enter the program: ");
                    String program = Console.ReadLine();
                    Console.Write("Please enter the desired password: ");
                    String password = hideAndEnterPassword();
                    changePassword(program, password);
                } else {
                    Console.WriteLine("ERROR: Invalid Command");
                }
            }
        }

        public void addPassword(String program, String password) {
            if (savedPasswords.addPassword(program, encryption.EncryptPassword(password))) {
                Console.WriteLine("SUCCESS: Password has been added");
            } else {
                Console.WriteLine("ERROR: A password already exists for the specified program.");
            }
        }

        public void changePassword(String program, String password) {
            savedPasswords.changePassword(program, password);
            if (!savedPasswords.removePassword(program)) {
                Console.WriteLine("ERROR: Program not found");
            } else {
                Console.WriteLine("SUCCESS: Password has been changed");
            }
        }
        
        public void getPassword(String program) {
            if (!savedPasswords.findCorrespondingPassword(program, encryption)) {
                Console.WriteLine("ERROR: Program not found");
            }
        }

        public void removePassword(String program) {
            if (!savedPasswords.removePassword(program)) {
                Console.WriteLine("ERROR: Program not found");
            } else {
                Console.WriteLine("SUCCESS: Password has been deleted");
            }
        }

        public String hideAndEnterPassword() {
            String password = String.Empty;
            ConsoleKey key;
            do {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine("");
            return password;
        }

        public class Password
        {
            public String Program { get; set; }
            public String EncryptedPassword { get; set; }
        }
    }
}