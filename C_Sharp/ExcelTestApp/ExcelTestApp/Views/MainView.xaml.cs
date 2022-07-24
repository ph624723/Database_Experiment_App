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
using Access_Test_Project.Model;
using Access_Test_Project.Model.Entities;
using ExcelTestApp.Utilities;
using ExcelTestApp.Views.Controls;
using ExcelTestApp.Views.Dialogs;
using MaterialDesignThemes.Wpf;

namespace ExcelTestApp.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public User CurrentUser { get; set; }
        public bool IsDialogOpen { get; set; }

        public MainView()
        {
            UserFilters = new ObservableCollection<FilterItem<User>>();
            InitializeComponent();
            this.DataContext = this;
        }
        
        public void Refresh()
        {
            using (var unit = new DataService())
            {
                RefreshUsers(unit);
                RefreshDepartments(unit);
                RefreshTpls(unit);
            }
        }

        #region Users

        private ObservableCollection<User> _users;
        public ObservableCollection<User> UsersToShow
        {
            get => new ObservableCollection<User>(_users?.Where(x => UserFilters.All(y => y.AllowedEntities.Contains(x))) ?? new List<User>());
            set => _users = value;
        }

        #region UserFilter

        
        public ObservableCollection<FilterItem<User>> UserFilters { get; set; }

        public void AddNewUserFilter(string selectedField, string selectedKind, List<int> ids)
        {
            bool kindBool;
            if (selectedKind.Equals("Nur Elemente mit:")) kindBool = true;
            else kindBool = false;

            IEnumerable<User> allowedUsers = new List<User>();
            allowedUsers = _users.Where(x => ids.Contains(x.Id) == kindBool);

            FilterItem<User> item = new FilterItem<User>(allowedUsers, new Chip()
            {
                Content = selectedField,
                IsDeletable = true,
                Margin = new Thickness(10),
            });
            item.DeleteClick += UserFilterItemOnDeleteClick;
            UserFilterChips.Children.Add(item.Chip);
            UserFilters.Add(item);
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("UsersToShow"));
        }

        private void UserFilterItemOnDeleteClick(object sender, EventArgs e)
        {
            FilterItem<User> origin = sender as FilterItem<User>;
            if (origin == null) return;

            UserFilterChips.Children.Remove(origin.Chip);
            UserFilters.Remove(origin);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsersToShow"));
        }
        
        public void GetFilterValues(string selectedField)
        {
            UserFilter.AvailableValues = new List<MultiselectStringListItem>();
            switch (selectedField)
            {
                case "Benutzername":
                    _users.ToList().ForEach(x => UserFilter.AvailableValues.Add(new MultiselectStringListItem(x.Id,x.Name)));
                    break;
                case "E-Mail Adresse":
                    _users.ToList().ForEach(x => UserFilter.AvailableValues.Add(new MultiselectStringListItem(x.Id, x.Mail)));
                    break;
                case "Rollen":
                    UserToRoleListItemConverter con  = new UserToRoleListItemConverter();
                    _users.ToList().ForEach(x => UserFilter.AvailableValues.AddRange((List<MultiselectStringListItem>)con.Convert(x,x.GetType(),"",null)));
                    break;
            }
        }



        #endregion

        public User SelectedUser { get; set; }

        public void RefreshUsers(DataService dataService = null, bool removeFilters = true)
        {
            if (removeFilters)
            {
                UserFilterChips.Children.Clear();
                UserFilters.Clear();
            }
            
            if (dataService!=null) UsersToShow = dataService.Users.GetAll();
            else
            {
                using (var unit = new DataService())
                {
                    UsersToShow = unit.Users.GetAll();
                }
            }

            var availableFields = new ObservableCollection<string>();
            foreach (var column in UsersDataGrid.Columns)
            {
                availableFields.Add(column.Header.ToString());
            }
            var availableKinds = new ObservableCollection<string>();
            availableKinds.Add("Nur Elemente mit:");
            availableKinds.Add("Alle Elemente außer:");
            UserFilter.InitializeData(AddNewUserFilter, GetFilterValues, availableFields, availableKinds, OpenFilterDialog);
        }

        private void OpenFilterDialog(FilterDialog dia)
        {
            dia.Finished += FilterDialogOnFinished;

            MainDialogHost.DialogContent = dia;

            IsDialogOpen = true;
        }

        private void FilterDialogOnFinished(object sender, EventArgs e)
        {
            MainDialogHost.DialogContent = null;
            IsDialogOpen = false;
        }

        private void EditUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserDialog dia  = new UserDialog();
            dia.User = SelectedUser;
            dia.Finished+= UserDialogOnFinished;
            MainDialogHost.DialogContent = dia;

            IsDialogOpen = true;
        }

        private void AddUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserDialog dia = new UserDialog();
            dia.Finished += UserDialogOnFinished;
            MainDialogHost.DialogContent = dia;

            IsDialogOpen = true;
        }

        private void UserDialogOnFinished(object sender, EventArgs e)
        {
            MainDialogHost.DialogContent = null;
            IsDialogOpen = false;
            if (sender as bool? ?? false) RefreshUsers();
        }

        private void RemoveUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveDialog dia = new RemoveDialog();
            dia.Finished += RemoveUserDialogOnFinished;

            MainDialogHost.DialogContent = dia;
            IsDialogOpen = true;
        }

        private void RemoveUserDialogOnFinished(object sender, EventArgs e)
        {
            MainDialogHost.DialogContent = null;
            IsDialogOpen = false;
            if (sender as bool? ?? false)
            {
                using (var unit = new DataService())
                {
                    unit.Users.Remove(SelectedUser.Id);
                    RefreshUsers(unit, false);
                }
            }
        }

        #endregion

        #region Departments

        public ObservableCollection<Department> DepartmentsToShow { get; set; }
        public Department SelectedDepartment { get; set; }

        public bool DeleteDepartmentEnabled
        {
            get
            {
                using (var unit = new DataService())
                {
                    return !unit.Tpls.GetAll().Any(x => x.Department.Equals(SelectedDepartment?.Id));
                }
            }
        }
        
        public void RefreshDepartments(DataService dataService = null)
        {
            if (dataService != null) DepartmentsToShow = dataService.Departments.GetAll();
            else
            {
                using (var unit = new DataService())
                {
                    DepartmentsToShow = unit.Departments.GetAll();
                }
            }
            
        }

        private void EditDepartmentButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveDepartmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveDialog dia = new RemoveDialog();
            dia.Finished += RemoveDepartmentDialogOnFinished;

            MainDialogHost.DialogContent = dia;
            IsDialogOpen = true;
        }

        private void RemoveDepartmentDialogOnFinished(object sender, EventArgs e)
        {
            MainDialogHost.DialogContent = null;
            IsDialogOpen = false;
            using (var unit = new DataService())
            {
                if ((sender as bool? ?? false) &&
                    !unit.Tpls.GetAll().Any(x => x.Department.Equals(SelectedDepartment.Id)))
                {
                    unit.Departments.Remove(SelectedDepartment.Id);
                    RefreshDepartments(unit);
                }
            }
        }


        private void AddDepartmentButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region TPLs

        public ObservableCollection<Tpl> TplsToShow { get; set; }
        public Tpl SelectedTpl { get; set; }

        public void RefreshTpls(DataService dataService = null)
        {
            if (dataService != null) TplsToShow = dataService.Tpls.GetAll();
            else
            {
                using (var unit = new DataService())
                {
                    TplsToShow = unit.Tpls.GetAll();
                }
            }
            
        }

        private void EditTplButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveTplButton_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveDialog dia = new RemoveDialog();
            dia.Finished += RemoveTplDialogOnFinished;

            MainDialogHost.DialogContent = dia;
            IsDialogOpen = true;
        }

        private void RemoveTplDialogOnFinished(object sender, EventArgs e)
        {
            MainDialogHost.DialogContent = null;
            IsDialogOpen = false;
            if (sender as bool? ?? false)
            {
                using (var unit = new DataService())
                {
                    unit.Tpls.Remove(SelectedTpl.Id);
                    RefreshTpls(unit);
                }
            }
        }

        private void AddTplButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        #endregion


    }
}
