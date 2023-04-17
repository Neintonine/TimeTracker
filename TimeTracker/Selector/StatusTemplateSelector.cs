using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimeTracker.Types;

namespace TimeTracker.Selector
{
    public class StatusTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Empty { get; set; }
        public DataTemplate NoStatus { get; set; }
        public DataTemplate DontBook { get; set; }
        public DataTemplate Booked { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (
                item != null &&
                item is TimeEntry entry
                )
            {

                if (entry.BookingStatus == TimeEntry.Status.NoStatus)
                {
                    return NoStatus;
                }

                if (entry.BookingStatus == TimeEntry.Status.DontBook)
                {
                    return DontBook;
                }

                if (entry.BookingStatus == TimeEntry.Status.Booked) 
                {
                    return Booked;
                }
            }

            return Empty;
        }
    }
}
