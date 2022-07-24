using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    /// <summary>
    /// Defining the rights of an Account it is Assigned to
    /// </summary>
    public class Role:DatabaseObject
    {
        public string Name { get; set; }

        public override string ToShow => Name;
    }
}
