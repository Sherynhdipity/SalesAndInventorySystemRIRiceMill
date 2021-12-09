using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Commands;
using R1RiceMill.Core;
using R1RiceMill.Data;
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels
{
    public class LoginViewModel : ObservableObject
    {

        public LoginViewModel()
        {
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private User _loggedInUser;

        public User LoggedInUser
        {
            get { return _loggedInUser; }
            set { Set(ref _loggedInUser, value); }
        }

        private AsyncRelayCommand _Login;
        public AsyncRelayCommand LoginCommand => _Login ?? (_Login = new AsyncRelayCommand(
            async () =>
            {
                User user = null;
                using (var db = new DatabaseContext())
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Username == Username && u.Password == Password);
                }

                if (user is not null)
                {
                    if (user.IsActive)
                    {
                        WindowManager.ShowMain(user);
                    }
                    else
                    {
                        MessageBox.Show("User is inactive. Contact your administrator.", "Login Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Retry", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }));

    }
}
