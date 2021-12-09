using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Collections;
using NittyGritty.Commands;
using NittyGritty.ViewModels;
using R1RiceMill.Core;
using R1RiceMill.Data;
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels.Admin
{
    public class UsersViewModel : ViewModelBase
    {
        public UsersViewModel()
        {
            Users = new DynamicCollection<User>(Enumerable.Empty<User>(), null, u => u.FirstName, true);
        }

        public DynamicCollection<User> Users { get; }

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user?.CancelEdit();
                value?.BeginEdit();
                Set(ref _user, value);
            }
        }

        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set { Set(ref _newPassword, value); }
        }

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { Set(ref _confirmPassword, value); }
        }

        private AsyncRelayCommand _AddUser;
        public AsyncRelayCommand AddUserCommand => _AddUser ?? (_AddUser = new AsyncRelayCommand(
            async () =>
            {
                var viewModel = new AddUserViewModel();
                var result = WindowManager.Show<AddUserViewModel, AdminWindow>(DialogKeys.AddUser, viewModel);
                if (result)
                {
                    var user = viewModel.User;
                    using (var db = new DatabaseContext())
                    {
                        db.Users.Add(user);
                        await db.SaveChangesAsync(); 
                    }
                    Users.Add(user);
                }
            }));

        private AsyncRelayCommand _Activate;
        public AsyncRelayCommand ActivateCommand => _Activate ?? (_Activate = new AsyncRelayCommand(
            async () =>
            {
                if (User is not null && !User.IsActive)
                {
                    User.IsActive = true;
                    User.EndEdit();
                    using (var db = new DatabaseContext())
                    {
                        db.Users.Update(User);
                        await db.SaveChangesAsync(); 
                    }
                }
            }));

        private AsyncRelayCommand _Deactivate;
        public AsyncRelayCommand DeactivateCommand => _Deactivate ?? (_Deactivate = new AsyncRelayCommand(
            async () =>
            {
                if (User is not null && User.IsActive)
                {
                    User.IsActive = false;
                    User.EndEdit();
                    using (var db = new DatabaseContext())
                    {
                        db.Users.Update(User);
                        await db.SaveChangesAsync(); 
                    }
                }
            }));

        private AsyncRelayCommand _Save;
        public AsyncRelayCommand SaveCommand => _Save ?? (_Save = new AsyncRelayCommand(
            async () =>
            {
                if (User != null)
                {
                    if (string.IsNullOrWhiteSpace(User.FirstName))
                    {
                        MessageBox.Show("First name should not be empty");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(User.LastName))
                    {
                        MessageBox.Show("Last name should not be empty");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(User.Username))
                    {
                        MessageBox.Show("Username should not be empty");
                        return;
                    }

                    if (!Enum.IsDefined(User.Role) || User.Role == Role.None)
                    {
                        MessageBox.Show("Role should only be Administrator, Cashier, or Inventory Clerk");
                        return;
                    }

                    if ((!string.IsNullOrWhiteSpace(NewPassword) || !string.IsNullOrWhiteSpace(ConfirmPassword)) && !string.Equals(NewPassword, ConfirmPassword))
                    {
                        MessageBox.Show("Passwords don't match");
                        return;
                    }

                    User.EndEdit();
                    using (var db = new DatabaseContext())
                    {
                        db.Users.Update(User);
                        await db.SaveChangesAsync(); 
                    }
                    User.BeginEdit();
                }
            }));
        
        private AsyncRelayCommand _Load;
        public AsyncRelayCommand LoadCommand => _Load ?? (_Load = new AsyncRelayCommand(
            async () =>
            {
                using (var db = new DatabaseContext())
                {
                    var users = await db.Users.ToListAsync();
                    Users.AddRange(users); 
                }
            }));

        public override async void LoadState(object parameter, Dictionary<string, object> state)
        {
            await LoadCommand.TryExecute();
        }

        public override void SaveState(Dictionary<string, object> state)
        {
            Users.Clear();
        }
    }
}
