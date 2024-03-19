using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MagazineManager.CmdDeveloperToolNS
{
    public static class CmdDeveloperToolUsers
    {
        private static string[] names = {
            "Adam", "Anna", "Michał", "Karolina", "Tomasz",
            "Magdalena", "Kamil", "Monika", "Piotr", "Patrycja",
            "Łukasz", "Kinga", "Krzysztof", "Natalia", "Marcin",
            "Justyna", "Bartosz", "Agata", "Mateusz", "Aleksandra"};

        private static string[] surnames = {
            "Kowalski", "Nowak", "Wiśniewski", "Wójcik", "Kowalczyk",
            "Kamiński", "Lewandowski", "Zielińska", "Szymański", "Woźniak",
            "Dąbrowski", "Kozłowski", "Jankowski", "Mazur", "Wojciechowski",
            "Kwiatkowski", "Kaczmarek", "Majewski", "Grabowski", "Olszewski"};

        private static string[] positions = {
            "Storekeeper", "Warehouse Manager", "Production Worker", "Machine Operator", "Logistics Specialist",
            "Cleaner", "Engineer", "Sales Manager", "Accountant", "IT Administrator",
            "Technician", "Quality Assurance Specialist", "Production Supervisor", "Financial Analyst", "Customer Advisor",
            "HR Manager", "Marketing Specialist", "Graphic Designer", "Office Clerk", "Distributor"};

        public static void addSimpleUser(string fullCommand)
        {
            string[] commandParts = fullCommand.Split(' ');

            if (commandParts.Length == 1)
            {
                Console.WriteLine("   [addUser-s error]: Login and password have not been written.");
                return;
            }
            else if (commandParts.Length == 2)
            {
                Console.WriteLine("   [addUser-s error]: Password has not been written.");
                return;
            }

            string command = commandParts[0];
            string login = commandParts[1];

            if (UserManagement.isLoginExist(login))
            {
                Console.WriteLine("   [addUser-s error]: This login is exist. You have to use another.");
                return;
            }

            SecureString password = PasswordManager.ConvertToSecureString(commandParts[2]);
            string name = GetRandomName();
            string surname = GetRandomSurname();
            string email = name+surname+"@email.com";
            string position = GetRandomPosition();
            int hierarchy = 10;
            bool[] permissions = new bool[3];
            permissions[0] = false;
            permissions[1] = false;
            permissions[2] = false;



            if (UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
            {
                Console.WriteLine("   [addUser-s]: The account has been created.");
            }
        }

        public static void addRandomUser(string fullCommand)
        {
            string[] commandParts = fullCommand.Split(' ');

            int amount = 1;

            if (commandParts.Length == 2)
            {
                if (int.TryParse(commandParts[1], out int number))
                {
                    if(number > 100)
                    {
                        if (!confirmationAnswer("Are you sure you want to create more than 100 accounts?")) return;     
                    }

                    amount = number;
                }
                else
                {
                    Console.WriteLine("   [addUser-r]: Argument is not a number.");
                    return;
                }
            }

            for (int i = 0; i <= amount; i++)
            {
                string login = GetRandomLogin();
                SecureString password = PasswordManager.ConvertToSecureString("qwerty");
                string name = GetRandomName();
                string surname = GetRandomSurname();
                string email = name + surname + "@email.com";
                string position = GetRandomPosition();
                int hierarchy = 10;
                bool[] permissions = new bool[3];
                permissions[0] = false;
                permissions[1] = false;
                permissions[2] = false;



                if (!UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
                {
                    Console.WriteLine($"   [addUser-r]: The account {i} has been not created.");
                }
            }

            Console.WriteLine($"   [addUser-r]: {amount} account/s has been created.");

        }

        private static string GetRandomName()
        {
            Random random = new Random();

            int randomNumber = random.Next(0, 19 + 1);

            return names[randomNumber];
        }

        private static string GetRandomSurname()
        {
            Random random = new Random();

            int randomNumber = random.Next(0, 19 + 1);

            return surnames[randomNumber];
        }

        private static string GetRandomPosition()
        {
            Random random = new Random();

            int randomNumber = random.Next(0, 19 + 1);

            return positions[randomNumber];
        }

        private static string GetRandomLogin()
        {
            Random random = new Random();

            string possibleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string login;

            while(true)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    int index = random.Next(possibleCharacters.Length);
                    builder.Append(possibleCharacters[index]);
                }

                string randomString = builder.ToString();

                if (!UserManagement.isLoginExist(randomString))
                {
                    login = randomString;
                    break;
                }
            }


            return login;
        }

        private static bool confirmationAnswer(string answer)
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
