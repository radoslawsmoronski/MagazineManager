using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;

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
                    case "showUser": CmdDeveloperToolUsers.showUserDetailsByLogin(getCommandAttributes(fullCommand)); break;
                    case "deleteUser": CmdDeveloperToolUsers.deleteUserByLogin(getCommandAttributes(fullCommand)); break;
                    case "addUser": CmdDeveloperToolUsers.addUser(getCommandAttributes(fullCommand)); break;
                    case "editUser": CmdDeveloperToolUsers.editUser(getCommandAttributes(fullCommand)); break;
                    case "clear": Console.Clear(); break;
                    case "exit": exit(); break;
                    default: Console.WriteLine(""); break;
                }
            }
        }

        private Dictionary<string, List<string>> getCommandAttributes(string fullCommand)
        {
            string pattern = @"(?:(?<=^|\s)'([^']+)'|(?<!'[^']*)-(\S+))";

            MatchCollection commandAttributes = Regex.Matches(fullCommand, pattern);

            Dictionary<string, List<string>> flagText = new Dictionary<string, List<string>>();
            flagText["Text"] = new List<string>();
            flagText["Flag"] = new List<string>();

            foreach (Match match in commandAttributes)
            {
                if (match.Groups[1].Success)
                {
                    flagText["Text"].Add(match.Groups[1].Value);
                }
                else if (match.Groups[2].Success)
                {
                    flagText["Flag"].Add(match.Groups[2].Value);
                }
            }

            return flagText;
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

                    var categories = from category in doc.Descendants("category")
                                     select new
                                     {
                                         Name = category.Attribute("name").Value,
                                         Commands = from command in category.Descendants("command")
                                                    select new
                                                    {
                                                        Name = command.Attribute("name").Value,
                                                        SDescription = command.Element("short-description").Value
                                                    }
                                     };

                    foreach (var category in categories)
                    {
                        Console.WriteLine($"\n   {category.Name} commands");

                        foreach (var command in category.Commands)
                        {
                            Console.WriteLine($"   {command.Name} - {command.SDescription}");
                        }
                    }

                    Console.WriteLine(
                        "\n   if you need more information about a given command,\n" +
                        "   such as arguments, use the help <command name> command,\n" +
                        "   e.g. help addUser-s");
                }
                else
                {
                    var commandDetails = (from command in doc.Descendants("command")
                                         where command.Attribute("name").Value == commandParts[1]
                                         select new
                                         {
                                             Name = command.Attribute("name").Value,
                                             LDescription = command.Element("long-description").Value
                                         }).FirstOrDefault();


                    Console.WriteLine($"   [help] {commandDetails.Name} - Command Details help information\n{commandDetails.LDescription}");

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
