using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if (item is not TimeEntry entry) return Empty;
            
            PropertyChangedEventHandler ev = null;
            ev = (sender, args) =>
            {
                if (args.PropertyName != "BookingStatus")
                {
                    return;
                }

                entry.PropertyChanged -= ev;

                ContentPresenter cp = (ContentPresenter)container;
                cp.ContentTemplateSelector = null;
                cp.ContentTemplateSelector = this;
            };
            entry.PropertyChanged += ev;
                
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

            return Empty;
        }
    }
}
