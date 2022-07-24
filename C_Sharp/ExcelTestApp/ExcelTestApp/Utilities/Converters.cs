using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Access_Test_Project.Model;
using ExcelTestApp.Views.Controls;
using Access_Test_Project.Model.Entities;

namespace ExcelTestApp.Utilities
{
    class UserToRoleListItemConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value?.GetType() != typeof(User)) return null;//Binding.DoNothing;

            List<MultiselectStringListItem> toReturn = new List<MultiselectStringListItem>();
            using (var unit = new DataService())
            {
                ((User)value).RoleIds.ToList()
                    .ForEach(id => toReturn.Add(new MultiselectStringListItem(((User)value).Id, unit.Roles.Get(id)?.Name ?? "")));
            }

            return toReturn;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    class UserRoleIdsToStringConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value?.GetType() != typeof(List<int>) || ((ICollection<int>)value).Count < 1) return null;//Binding.DoNothing;

            string toReturn = "";
            using (var unit = new DataService())
            {
                ((ICollection<int>) value).ToList()
                    .ForEach(id => toReturn += (unit.Roles.Get(id)?.Name ?? "") + ", ");
            }

            return toReturn.Substring(0, toReturn.Length - 2);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    class DepartmentIdToNameConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            using (var unit = new DataService())
            {
                return value != null ? unit.Departments.Get((int) value).Name : Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
