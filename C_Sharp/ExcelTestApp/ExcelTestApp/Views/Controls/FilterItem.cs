using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Access_Test_Project.Model;
using MaterialDesignThemes.Wpf;

namespace ExcelTestApp.Views.Controls
{
    public class FilterItem<TEntity> where TEntity:DatabaseObject
    {
        public ObservableCollection<TEntity> AllowedEntities { get; }
        public Chip Chip { get; }

        public FilterItem(IEnumerable<TEntity> allowed, Chip chip)
        {
            Chip = chip;
            AllowedEntities = new ObservableCollection<TEntity>(allowed);
            Chip.DeleteClick += ChipOnDeleteClick;
        }


        public event EventHandler DeleteClick;
        private void ChipOnDeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
