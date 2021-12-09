using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;
using R1RiceMill.Core;

namespace R1RiceMill.ViewModels.Admin
{
    public class AddUserViewModel : ObservableObject
    {

        private Role _role;

        public Role Role
        {
            get { return _role; }
            set
            {
                Set(ref _role, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                Set(ref _firstName, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private string _middleName;

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                Set(ref _middleName, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                Set(ref _lastName, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

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

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                Set(ref _confirmPassword, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            string.Equals(Password, ConfirmPassword) &&
            Enum.IsDefined(Role) &&
            Role != Role.None;

        public User User => IsValid ?
            new User
            {
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Username = Username,
                Password = Password,
                Role = Role
            } :
            null;

    }
}
