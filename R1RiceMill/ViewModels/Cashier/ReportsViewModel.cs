using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty.Commands;
using NittyGritty.ViewModels;
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels.Cashier
{
    public class ReportsViewModel : ViewModelBase
    {

        private DateTime? _startDate;

        public DateTime? StartDate
        {
            get { return _startDate; }
            set { Set(ref _startDate, value); }
        }

        private DateTime? _endDate;

        public DateTime? EndDate
        {
            get { return _endDate; }
            set { Set(ref _endDate, value); }
        }

        private AsyncRelayCommand _SalesReport;
        public AsyncRelayCommand SalesReportCommand => _SalesReport ?? (_SalesReport = new AsyncRelayCommand(
            async () =>
            {
                await ReportService.SalesReport(StartDate, EndDate);
            }));

        private AsyncRelayCommand _ReturnsReport;
        public AsyncRelayCommand ReturnsReportCommand => _ReturnsReport ?? (_ReturnsReport = new AsyncRelayCommand(
            async () =>
            {
                await ReportService.ReturnsReport(StartDate, EndDate);
            }));

        public override void LoadState(object parameter, Dictionary<string, object> state)
        {
            
        }

        public override void SaveState(Dictionary<string, object> state)
        {
            
        }
    }
}
