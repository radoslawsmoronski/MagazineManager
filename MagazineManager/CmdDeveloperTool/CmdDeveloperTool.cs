using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MagazineManager.CmdDeveloperToolNS
{
    public class CmdDeveloperTool
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        private bool stillUse = true;   

        public CmdDeveloperTool()
        {
            AllocConsole(); //Support Console

            while (stillUse)
            {
                Console.Write("MagazineManager.CmdDeveloperTool -> ");
                string fullCommand = Console.ReadLine().ToString();

                string[] commandParts = fullCommand.Split(' ');
                string command = commandParts[0];

                switch (command)
                {
                    case "addUser-s": CmdDeveloperToolUsers.addSimpleUser(fullCommand); break;
                    case "exit": exit(); break;
                    default: Console.WriteLine(""); break;
                }
            }
        }

        private void exit()
        {
            if(confirmationAnswer("Are you sure to exit the developer tool ?"))
            {
                stillUse = false;
                FreeConsole();
            }
        }

        private bool confirmationAnswer(string answer)
        {
            Console.WriteLine(
            $"   {answer}\n" +
            "   Write   Yes   to confirmation...");

            string confirmation = Console.ReadLine().ToString();
            if (confirmation == "Yes") return true;

            Console.WriteLine("   You didn't accept.");
            return false;
        }

    }
}
