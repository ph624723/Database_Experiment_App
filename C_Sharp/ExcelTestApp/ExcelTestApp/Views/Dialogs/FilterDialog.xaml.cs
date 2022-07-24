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
using ExcelTestApp.Views.Controls;

namespace ExcelTestApp.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for FilterDialog.xaml
    /// </summary>
    public partial class FilterDialog : UserControl, INotifyPropertyChanged
    {
        public event EventHandler Finished;
        private Filter.AddNewFilterDelegate _addNewFilter;
        private string _fieldToFilter;
        private string _filterKind;
        private IMultiselectStringList _list;

        public FilterDialog(Filter.AddNewFilterDelegate addNewFilter, string fieldToFilter, string filterKind, List<MultiselectStringListItem> items) :this()
        {
            _addNewFilter=addNewFilter;
            _filterKind = filterKind;
            _fieldToFilter = fieldToFilter;
            _list = new MultiselectStringList();
            _list.Control = ListControl;
            _list.AvailableEntities = items;
        }

        public FilterDialog()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            _addNewFilter(_fieldToFilter,_filterKind , _list.SelectedEntites);
            Finished?.Invoke(true, EventArgs.Empty);
        }

        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            Finished?.Invoke(false, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
