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


    }
}
