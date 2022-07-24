using Access_Test_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access_Test_Project.Model.Entities
{
    public class ProductionLine: DatabaseObject
    {
        public ProductionLine()
        {
            ProcessStreamIds= new Collection<int>();
        }

        public string Name { get; set; }
        public int Tpl { get; set; }
        public int Plant { get; set; }
        public ICollection<int> ProcessStreamIds { get; set; }

        public override string ToShow => Name;
    }
}
