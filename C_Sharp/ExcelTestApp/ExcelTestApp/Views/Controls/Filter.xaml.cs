using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ExcelTestApp.Views.Dialogs;

namespace ExcelTestApp.Views.Controls
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool NewFilterOpen => NewFilterStage != 0;
        public Visibility NewFilterButtonVisible => NewFilterStage == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility AbortNewFilterButtonVisible => NewFilterStage > 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility SelectedKindComboBoxVisible =>
            NewFilterStage >= 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility SelectedValueComboBoxVisible =>
            NewFilterStage >= 3 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ConfirmNewFilterButtonVisible =>
            NewFilterStage >= 3 ? Visibility.Visible : Visibility.Collapsed;

        public string SelectedField { get; set; }
        public string SelectedKind { get; set; }
        public string SelectedValue { get; set; }

        public bool SelectedFieldComboBoxEnabled => NewFilterStage == 1;
        public bool SelectedKindComboBoxEnabled => NewFilterStage == 2;
        public bool ConfirmNewFilterButtonEnabled => SelectedValue != null;

        public ObservableCollection<string> AvailableFields { get; set; }
        public ObservableCollection<string> AvailableKinds { get; set; }
        public List<MultiselectStringListItem> AvailableValues { get; set; }

        private int _newFilterStage;

        public int NewFilterStage
        {
            get => _newFilterStage;
            set
            {
                if (value >= 0 && value <= 3)
                {
                    _newFilterStage = value;
                    if (value == 1 && SelectedFieldComboBox != null && SelectedKindComboBox != null) 
                    {
                        SelectedFieldComboBox.SelectedValue = null;
                        SelectedKindComboBox.SelectedValue = null;
                    }
                }
            }
        }

        public delegate void AddNewFilterDelegate(string fieldToFilter, string filterKind, List<int> ids);

        public delegate void GetFilterValuesDelegate(string field);

        public delegate void OpenDialogDelegate(FilterDialog dia);

        private AddNewFilterDelegate _addNewFilter;
        private GetFilterValuesDelegate _getFilterValues;
        private OpenDialogDelegate _openDialog;

        public bool DataInitialized { get; private set; }

        public Filter()
        {
            InitializeComponent();
            this.DataContext = this;
            DataInitialized = false;
            NewFilterStage = 0;

        }

        public void InitializeData(AddNewFilterDelegate addNewFilter, GetFilterValuesDelegate getFilterValues, ObservableCollection<string> availableFields, ObservableCollection<string> availableKinds, OpenDialogDelegate openDialog)
        {
            if (addNewFilter == null || getFilterValues == null || availableFields == null || availableKinds == null) return;
            DataInitialized = true;
            _addNewFilter = addNewFilter;
            _getFilterValues = getFilterValues;
            _openDialog = openDialog;
            AvailableFields = availableFields;
            AvailableKinds = availableKinds;
        }

        private void AbortNewFilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            NewFilterStage = 0;
            
        }

        private void NewFilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataInitialized)
            {
                NewFilterStage = 1;
            }
        }

        private void SelectedFieldComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedField != null && NewFilterStage == 1)
            {
                NewFilterStage = 2;
            }
        }

        private void SelectedKindComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedKind != null && NewFilterStage == 2)
            {
                _getFilterValues(SelectedField);
                NewFilterStage = 3;
                FilterDialog dia = new FilterDialog(_addNewFilter, SelectedField, SelectedKind, AvailableValues);
                dia.Finished+=DiaOnFinished;
                _openDialog(dia);
            }
        }

        private void DiaOnFinished(object sender, EventArgs e)
        {
            NewFilterStage = 0;
        }
    }
}
