using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
using DocumentFormat.OpenXml.Vml.Office;
using TimeTracker.Controls;
using TimeTracker.Types;
using Calendar = System.Windows.Controls.Calendar;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for DataEntry.xaml
    /// </summary>
    public partial class DataEntry : UserControl
    {
        private CollectionViewSource _viewSource;

        private string _bookingString;
        private string _dateString;

        public DataEntry()
        {
            InitializeComponent();
        }

        private void UpdateFilter()
        {
            ListCollectionView view = Data.ItemsSource as ListCollectionView;
            view.Filter -= Filter;
            view.Filter += Filter;
        }

        private bool Filter(object obj)
        {
            TimeEntry entry = (TimeEntry)obj;

            bool dateFilter = DateFilter(entry);
            bool bookingFilter = BookFilter(entry);

            return dateFilter && bookingFilter;
        }

        private bool DateFilter(TimeEntry entry)
        {
            System.Globalization.Calendar cal = new CultureInfo("de-de").Calendar;

            switch (_dateString)
            {
                case "Today":
                    return entry.Date.Equals(DateTime.Today);
                case "This Week":
                    return cal.GetWeekOfYear(entry.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday) ==
                           cal.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                case "This Month":
                    return entry.Date.Month == DateTime.Today.Month;


                default:
                    return true;
            }

            return true;
        }

        private bool BookFilter(TimeEntry timeEntry)
        {
            switch (_bookingString)
            {
                case "Not booked":
                    return timeEntry.BookingStatus == TimeEntry.Status.NoStatus;
                case "Will not be booked":
                    return timeEntry.BookingStatus == TimeEntry.Status.DontBook;
                case "Booked":
                    return timeEntry.BookingStatus == TimeEntry.Status.Booked;

                default:
                    return true;
            }
        }

        private void BookedFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _bookingString = BookedFilter.SelectedItem as string;

            UpdateFilter();
        }

        private void TimeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _dateString = TimeFilter.SelectedItem as string;

            UpdateFilter();
        }
    }
}
