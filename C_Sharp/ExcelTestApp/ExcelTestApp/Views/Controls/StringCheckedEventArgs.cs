using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTestApp.Views.Controls
{
    public class StringCheckedEventArgs : EventArgs
    {
        public string Text { get; }

        public StringCheckedEventArgs(string text)
        {
            Text = text;
        }
    }
}
