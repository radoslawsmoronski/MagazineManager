using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Linq;
using System.Threading;

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
            Thread consoleThread = new Thread(openCmdDevelopterTool);
            consoleThread.Start();
        }

        public void openCmdDevelopterTool()
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
                    case "help": helpCommand(fullCommand); break;
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

        private void helpCommand(string fullCommand)
        {
            string[] commandParts = fullCommand.Split(' ');

            string fileName = "helpCommand.xml";
            string projectDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\MagazineManager\\CmdDeveloperTool");
            string filePath = Path.Combine(projectDirectory, fileName);

            try
            {
                XDocument doc = XDocument.Load(filePath);

                if(commandParts.Length == 1)
                {
                    Console.WriteLine("   [help] Commands lits:");

                    foreach (XElement commandElement in doc.Descendants("command"))
                    {

                        string commandName = (string)commandElement.Attribute("name");

                        string shortDescription = (string)commandElement.Element("short-description");

                        Console.WriteLine($"   {commandName} - {shortDescription}");
                    }

                    Console.WriteLine(
                        "\n   if you need more information about a given command,\n" +
                        "   such as arguments, use the help <command name> command,\n" +
                        "   e.g. help addUser-s");
                }
                else
                {
                    foreach (XElement commandElement in doc.Descendants("command"))
                    {
                        string commandName = (string)commandElement.Attribute("name");

                        if (commandName == commandParts[1])
                        {
                            string longDescription = (string)commandElement.Element("long-description");

                            Console.WriteLine($"   [help] {commandName} - Command Details help information\n{longDescription}");

                            return;
                        }
                    }

                    Console.WriteLine("   [help error]: This command doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("   [help error]: " + ex.Message);
            }
        }

    }
}
