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
using Access_Test_Project.Model;
using Access_Test_Project.Model.Entities;

namespace ExcelTestApp.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : UserControl, INotifyPropertyChanged
    {
        public event EventHandler Finished;

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                if(value?.Name != null) UserName = value.Name;
                if (value?.Mail != null) UserMailText = value.Mail;
                if (value?.Password != null) PasswordBox1.Password = value.Password;
                if (value?.Password != null) PasswordBox2.Password = value.Password;
                if (value == null)
                {
                    UserName = "";
                    UserMailText = "";
                    PasswordBox1.Password = "";
                    PasswordBox2.Password = "";
                }
            }
        }
        public bool HasUser => User != null;

        public string UserName { get; set; }
        public string UserMailText { get; set; }
        private string _userMail => UserMailText.Contains("@") ? UserMailText + "" : UserMailText + "@mail.de";


        public UserDialog()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AbortUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            User = null;
            Finished?.Invoke(false, EventArgs.Empty);
        }

        private void ConfirmUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password.Equals(PasswordBox2.Password) && PasswordBox1.Password.Length>=6)
            {
                if (HasUser)
                {
                    User.Password = PasswordBox1.Password;
                    User.Mail = _userMail;
                    User.Name = UserName;
                }
                else
                {
                    User = new User
                    {
                        Name = UserName,
                        Mail = _userMail,
                        Password = PasswordBox1.Password,
                    };
                }

                using (var unit = new DataService())
                {
                    unit.Users.Add(User);
                }

                User = null;
                Finished?.Invoke(true, EventArgs.Empty);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
