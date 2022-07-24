using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access_Test_Project.Model;

namespace ExcelTestApp.Views.Controls
{
    public interface IMultiselectList<TEntity> where TEntity:DatabaseObject
    {
        List<TEntity> SelectedEntites { get; }
        MultiselectListControl Control { get; set; }
        List<TEntity> AvailableEntities { set; }
    }

    public class MultiselectList<TEntity> : INotifyPropertyChanged, IMultiselectList<TEntity> where TEntity : DatabaseObject
    {
        private ObservableCollection<MultiselectListItem<TEntity>> _itemsToChooseFrom;

        private bool _invokingMyself = false;

        public ObservableCollection<MultiselectListItem<TEntity>> ItemsToShow
        {
            get => new ObservableCollection<MultiselectListItem<TEntity>>(_itemsToChooseFrom?.Where(x => x.ToDisplay.Contains("a") || true) ?? new List<MultiselectListItem<TEntity>>());
            set
            {
                _itemsToChooseFrom = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsAllItems3Selected"));
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

        public List<TEntity> SelectedEntites => (from MultiselectListItem<TEntity> item in ItemsToShow.Where(x => x.IsSelected)
            select item.Entity).ToList();

        private MultiselectListControl _control;
        public MultiselectListControl Control
        {
            get => _control;
            set
            {
                _control = value;
                _control.DataContext = this;
                _control.AllCheckbox.DataContext = this;
            }
        }

        public List<TEntity> AvailableEntities
        {
            set
            {
                var toSet = new ObservableCollection<MultiselectListItem<TEntity>>();
                value.ForEach(x => toSet.Add(new MultiselectListItem<TEntity>()
                {
                    Entity = x
                }));
                ItemsToShow = toSet;
            }
        }

        public MultiselectList()
        {
           _itemsToChooseFrom = new ObservableCollection<MultiselectListItem<TEntity>>();
            Control = new MultiselectListControl();
            Control.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
