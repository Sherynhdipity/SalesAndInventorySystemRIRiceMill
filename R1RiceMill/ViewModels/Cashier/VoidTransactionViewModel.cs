using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.ViewModels.Cashier
{
    public class VoidTransactionViewModel : ObservableObject
    {

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                Set(ref _username, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                Set(ref _password, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        public async Task<bool> AdminLogin()
        {
            User user = null;
            using (var db = new DatabaseContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Username == Username && u.Password == Password);
            }

            if (user is not null)
            {
                if (!user.IsActive)
                {
                    MessageBox.Show("User is inactive. Contact your administrator.", "Login Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                if (user.Role != Role.Administrator)
                {
                    MessageBox.Show("User must be an administrator. Contact your administrator.", "Login Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                return true;
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Retry", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
