using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class User : ObservableObject, IEditableObject
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        private string _middleName;

        public string MiddleName
        {
            get { return _middleName; }
            set { Set(ref _middleName, value); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
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

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { Set(ref _isActive, value); }
        }

        private Role _role;

        public Role Role
        {
            get { return _role; }
            set { Set(ref _role, value); }
        }

        [NotMapped]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        private bool isEditing = false;
        private User backup = null;

        public void BeginEdit()
        {
            if (isEditing) return;
            isEditing = true;
            backup = new User();
            backup.Id = Id;
            backup.Role = Role;
            backup.FirstName = FirstName;
            backup.MiddleName = MiddleName;
            backup.LastName = LastName;
            backup.Username = Username;
            backup.Password = Password;
            backup.IsActive = IsActive;
        }

        public void CancelEdit()
        {
            if (!isEditing) return;
            isEditing = false;
            Id = backup.Id;
            Role = backup.Role;
            FirstName = backup.FirstName;
            MiddleName = backup.MiddleName;
            LastName = backup.LastName;
            Username = backup.Username;
            Password = backup.Password;
            IsActive = backup.IsActive;
        }

        public void EndEdit()
        {
            if (!isEditing) return;
            isEditing = false;
            backup = null;
        }
    }
}
