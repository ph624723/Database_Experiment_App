using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access_Test_Project.Model;

namespace ExcelTestApp.Views.Controls
{
    public class MultiselectListItem<TEntity> where TEntity: DatabaseObject
    {
        public TEntity Entity { get; set; }
        public string ToDisplay => Entity?.ToShow ?? "";
        public bool IsSelected { get; set; }
    }
}
