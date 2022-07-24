using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelTestApp.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for RemoveDialog.xaml
    /// </summary>
    public partial class RemoveDialog : UserControl, INotifyPropertyChanged
    {
        public event EventHandler Finished;

        public RemoveDialog()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            Finished?.Invoke(false,EventArgs.Empty);
        }

        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            Finished?.Invoke(true, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
