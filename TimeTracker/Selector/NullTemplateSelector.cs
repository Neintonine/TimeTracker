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
    public class NullTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (
                item != null &&
                item is TimeEntry
                ) {
                return filledTemplate;
            }

            return emptyTemplate;
        }

        public DataTemplate emptyTemplate { get; set; }
        public DataTemplate filledTemplate { get; set; }
    }
}
