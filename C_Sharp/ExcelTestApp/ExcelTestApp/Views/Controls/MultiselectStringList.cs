using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Access_Test_Project.Model;

namespace ExcelTestApp.Views.Controls
{
    public interface IMultiselectStringList
    {
        List<int> SelectedEntites { get; }
        MultiselectListControl Control { get; set; }
        List<MultiselectStringListItem> AvailableEntities { set; }
    }

    public class MultiselectStringList : INotifyPropertyChanged, IMultiselectStringList
    {
        private ObservableCollection<MultiselectStringListItem> _itemsToChooseFrom;

        public ObservableCollection<MultiselectStringListItem> ItemsToShow
        {
            get => new ObservableCollection<MultiselectStringListItem>(_itemsToChooseFrom?.Where(x => x.ToDisplay.Contains(SearchText ?? "")) ?? new List<MultiselectStringListItem>());
            set
            {
                _itemsToChooseFrom = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsAllItems3Selected"));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemsToShow"));
            }
        }

        public DatabaseObject SelectedItem { get; set; }

        public bool IsAllItems3Selected
        {
            get => true;//ItemsToShow.All(x => x.IsSelected);
            set
            {
                if (ItemsToShow.All(x => x.IsSelected))
                {
                    foreach (var item in ItemsToShow)
                    {
                        item.IsSelected = false;
                    }
                }
                else
                {
                    foreach (var item in ItemsToShow)
                    {
                        item.IsSelected = true;
                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemsToShow"));
            }
        }

        public List<int> SelectedEntites {
            get
            {
                List<int> toReturn = new List<int>();
                foreach (var item in _itemsToChooseFrom)
                {
                    if(item.IsSelected) toReturn.AddRange(item.Ids);
                }
                return toReturn;
            }
        }

        private MultiselectListControl _control;
        public MultiselectListControl Control
        {
            get => _control;
            set
            {
                _control = value;
                _control.DataContext = this;
                _control.AllCheckbox.DataContext = this;
                _control.SearchTextBox.TextChanged += SearchTextBoxOnTextChanged;
            }
        }

        private void SearchTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchText = Control.SearchTextBox.Text;
        }

        public List<MultiselectStringListItem> AvailableEntities
        {
            set
            {
                var toAdd = new ObservableCollection<MultiselectStringListItem>();
                foreach (var newItem in value)
                {
                    foreach (string displayItem in newItem.ToDisplay.Split(','))
                    {
                        MultiselectStringListItem item;
                        if ((item = toAdd.FirstOrDefault(x => x.ToDisplay.Equals(newItem.ToDisplay))) != null)
                        {
                            item.Ids.AddRange(newItem.Ids);
                        }
                        else
                        {
                            toAdd.Add(newItem);
                        }
                    }
                }
                ItemsToShow = toAdd;
            } 
        }

        public MultiselectStringList()
        {
           _itemsToChooseFrom = new ObservableCollection<MultiselectStringListItem>();
            Control = new MultiselectListControl();

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
