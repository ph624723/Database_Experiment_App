using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access_Test_Project.Model;

namespace ExcelTestApp.Views.Controls
{
    public class MultiselectStringListItem
    {
        public MultiselectStringListItem()
        {
            Ids = new List<int>();
        }
        public MultiselectStringListItem(int id, string toDisplay) :this()
        {
            Ids.Add(id);
            ToDisplay=toDisplay;
        }

        public List<int> Ids { get; set; }
        public string ToDisplay { get; set; }
        public bool IsSelected { get; set; }
    }
}
