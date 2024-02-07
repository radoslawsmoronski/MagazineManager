using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineManager
{
    public class OtherUser
    {
        public int Id { get; set; }
        public string Login {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public int Hierarchy { get; set; }
        public bool CanAddUsers { get; set; }
        public bool CanDeleteUsers { get; set; }
        public bool CanEditUsers { get; set; }

        public OtherUser(int id, string login, string name, string surname, string email, string position, int hierarchy, bool canAddUsers, bool canDeleteUsers, bool canEditUsers)
        {
            Id = id;
            Login = login;
            Name = name;
            Surname = surname;
            Email = email;
            Position = position;
            Hierarchy = hierarchy;
            CanAddUsers = canAddUsers;
            CanDeleteUsers = canDeleteUsers;
            CanEditUsers = canEditUsers;
        }
    }
}
