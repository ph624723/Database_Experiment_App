using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class User: DatabaseObject
    {
        public User()
        {
            RoleIds= new Collection<int>();
        }

        public User(string name) : this()
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public ICollection<int> RoleIds { get; set; }

        public string NameAndMail 
        { 
            get
            { 
                return Name + "\t(" + Mail + ")";
            }
        }

        public override string ToShow => Name;
    }
}
