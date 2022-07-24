using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;
using WPFLocalizeExtension.Engine;
using Excel = Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;
using Access_Test_Project.Model;
using Access_Test_Project.Model.Entities;

namespace ExcelTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentUserText => MainviewEnabled? "Angemeldet als: \t" + SelectedUser?.NameAndMail ?? "Nicht angemeldet" : "";

        public MainWindow()
        {
            using (var unit = new DataService(AppDomain.CurrentDomain.BaseDirectory + @"..\..\test1.mdb"))//"C:\\Users\\Public\\test1.mdb"))//@"C:\Users\Public\test1.mdb"))
            {

            }
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(this);
            Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            InitializeComponent();
            this.DataContext = this;
            this.Width = screen.WorkingArea.Width;
            this.Height = screen.WorkingArea.Height;
            LocalizeDictionary.Instance.Culture = Thread.CurrentThread.CurrentCulture;
            OpenLogin();

        }

        #region Loginview

        public bool IsLoginDrawerOpen { get; set; }
        public bool MainviewEnabled { get; set; }
        public Visibility LoginPanelVisible { get; set; }
        public Visibility NewUserPanelVisible { get; set; }

        public ObservableCollection<User> SelectableUsers { get; set; }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (MainView != null) MainView.CurrentUser = value;
            }

        }

        public string NewUserName { get; set; }
        public string NewUserMail { get; set; }

        private void OpenLogin()
        {
            using (var unit = new DataService())
            {
                SelectableUsers =  unit.Users.GetAll();
            }
            IsLoginDrawerOpen = true;
            MainviewEnabled = false;
            LoginPanelVisible = Visibility.Visible;
            NewUserPanelVisible = Visibility.Collapsed;
        }

        private void CloseLogin()
        {
            IsLoginDrawerOpen = false;
            MainviewEnabled = true;
            MainView.Refresh();
        }

        private void ConfirmLoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null && PasswordBox.Password.Equals(SelectedUser.Password))
            {
                CloseLogin();
            }
        }

        private void StartNewUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoginPanelVisible = Visibility.Collapsed;
            NewUserPanelVisible = Visibility.Visible;
        }

        private void AbortNewUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoginPanelVisible = Visibility.Visible;
            NewUserPanelVisible = Visibility.Collapsed;
        }

        private void ConfirmNewUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewUserMail != null && NewUserMail != "" && NewUserName != null && NewUserName.Length > 5 && NewPasswordBox1.Password.Length>7 && NewPasswordBox2.Password.Length>7 && NewPasswordBox1.Password.Equals(NewPasswordBox2.Password))
            {
                string toAddMail = "";
                if (NewUserMail.Contains("@")) toAddMail += NewUserMail;
                else toAddMail = NewUserMail + "@mail.de";

                var newUser = new User()
                {
                    Name = NewUserName,
                    Mail = toAddMail,
                    Password = NewPasswordBox1.Password,
                };
                

                using (var unit = new DataService())
                {
                    newUser.RoleIds.Add(unit.Roles.FirstOrDefault(x => x.Name.Equals("Basis")).Id);

                    unit.Users.Add(newUser);
                }

                NewUserMail = "";
                NewUserName = "";
                NewPasswordBox1.Password = "";
                NewPasswordBox2.Password = "";

                OpenLogin();
            }
        }
        #endregion

        private void ChangeUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenLogin();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            
        }

        private void SaveAsButton_OnClick(object sender, RoutedEventArgs e)
        {
            /*Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = DataService.FileName; // Default file name
            dlg.DefaultExt = ".xlsm"; // Default file extension
            dlg.Filter = "Excel Workbook mit Makros|*.xlsm";

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                DataService.SaveAs(dlg.FileName);
            }*/
        }

        private void ChangeToGerman_OnClick(object sender, RoutedEventArgs e)
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("de-DE");
        }

        private void ChangeToEnglish_OnClick(object sender, RoutedEventArgs e)
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en");
        }
    }
}
