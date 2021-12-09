using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class Customer : ObservableObject, IEditableObject
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
            set
            {
                Set(ref _firstName, value);
                RaisePropertyChanged(nameof(FullName));
            }
        }

        private string _middleName;

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                Set(ref _middleName, value);
                RaisePropertyChanged(nameof(FullName));
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                Set(ref _lastName, value);
                RaisePropertyChanged(nameof(FullName));
            }
        }

        private string _contactNo;

        public string ContactNo
        {
            get { return _contactNo; }
            set { Set(ref _contactNo, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        private DateTime _birthday;

        public DateTime Birthday
        {
            get { return _birthday; }
            set { Set(ref _birthday, value); }
        }

        private ObservableCollection<Transaction> transactions;
        public ObservableCollection<Transaction> Transactions { get => transactions ?? (transactions = new ObservableCollection<Transaction>()); set => transactions = value; }

        [NotMapped]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [NotMapped]
        public int Age
        {
            get
            {
                var now = DateTime.Now;
                var a = (now.Year * 100 + now.Month) * 100 + now.Day;
                var b = (Birthday.Year * 100 + Birthday.Month) * 100 + Birthday.Day;

                return (a - b) / 10000;
            }
        }

        private bool isEditing = false;
        private Customer backup = null;

        public void BeginEdit()
        {
            if (isEditing) return;
            isEditing = true;
            backup = new Customer();
            backup.FirstName = FirstName;
            backup.MiddleName = MiddleName;
            backup.LastName = LastName;
            backup.ContactNo = ContactNo;
            backup.Address = Address;
            backup.Birthday = Birthday;
        }

        public void CancelEdit()
        {
            if (!isEditing) return;
            isEditing = false;
            FirstName = backup.FirstName;
            MiddleName = backup.MiddleName;
            LastName = backup.LastName;
            ContactNo = backup.ContactNo;
            Address = backup.Address;
            Birthday = backup.Birthday;
        }

        public void EndEdit()
        {
            if (!isEditing) return;
            isEditing = false;
            backup = null;
        }

    }
}
