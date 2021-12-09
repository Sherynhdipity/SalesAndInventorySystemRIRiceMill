using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class Product : ObservableObject
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private string _variety;

        public string Variety
        {
            get { return _variety; }
            set { Set(ref _variety, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        private ObservableCollection<Batch> batches;
        public ObservableCollection<Batch> Batches { get => batches ?? (batches = new ObservableCollection<Batch>()); set => batches = value; }

        [NotMapped]
        public double AvailableStock => Batches.Sum(b => b.AvailableStock);
    }
}
