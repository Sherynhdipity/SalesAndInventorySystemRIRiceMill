using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using R1RiceMill.Core;

namespace R1RiceMill.Services
{
    public static class WindowManager
    {
        public static void ShowMain(User user)
        {
            switch (user.Role)
            {
                case Role.Administrator:
                    Application.Current.MainWindow.Hide();
                    var adminWindow = new AdminWindow(user);
                    adminWindow.Show();
                    break;

                case Role.Cashier:
                    Application.Current.MainWindow.Hide();
                    var cashierWindow = new CashierWindow(user);
                    cashierWindow.Show();
                    break;

                case Role.InventoryClerk:
                    Application.Current.MainWindow.Hide();
                    var inventoryWindow = new InventoryWindow(user);
                    inventoryWindow.Show();
                    break;

                case Role.None:
                default:
                    MessageBox.Show("Invalid Username or Password", "Retry", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        private static readonly Dictionary<string, Type> _dialogsByKey = new Dictionary<string, Type>();

        public static void Configure(string key, Type value)
        {
            lock (_dialogsByKey)
            {
                if (_dialogsByKey.ContainsKey(key))
                {
                    throw new ArgumentException($"This key is already used: {key}");
                }

                if (_dialogsByKey.Any(p => p.Value == value))
                {
                    throw new ArgumentException(
                        "This dialog is already configured with key " + _dialogsByKey.First(p => p.Value == value).Key);
                }

                _dialogsByKey.Add(
                    key,
                    value);
            }
        }

        public static bool Show<T, TParent>(string key, T parameter, Func<T, Task> openedCallback = null) where TParent : Window
        {
            object uObject = null;
            lock (_dialogsByKey)
            {
                if (!_dialogsByKey.ContainsKey(key))
                {
                    throw new ArgumentException($"No such key: {key}. Did you forget to call DialogService.Configure?");
                }
                uObject = Activator.CreateInstance(_dialogsByKey[key]);
            }
            if (uObject is Window dialog)
            {
                dialog.Owner = Application.Current.Windows.OfType<TParent>().FirstOrDefault();
                dialog.DataContext = parameter;
                if (openedCallback != null)
                {
                    dialog.Loaded += async (s, e) => await openedCallback(parameter);
                }
                var result = dialog.ShowDialog();
                return result == true;
            }
            return false;
        }
    }
}
