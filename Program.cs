using PasswordFunctions;

namespace Password_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            launch();
        }

        public static void launch() {
            new ManagerFunctions().startUpAndRun();
        }
    }
}
