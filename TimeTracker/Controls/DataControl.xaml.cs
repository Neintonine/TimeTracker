using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTracker.Types;

namespace TimeTracker.Controls
{
    /// <summary>
    /// Interaction logic for DataControl.xaml
    /// </summary>
    public partial class DataControl : DataGrid
    {
        public DataControl()
        {
            InitializeComponent();

            CurrentCellChanged += _data_CurrentCellChanged;
            //_data.RowEditEnding += Data_RowEditEnding;
        }

        private void _data_CurrentCellChanged(object sender, EventArgs e)
        {
            CommitEdit();
        }

        private void Data_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
            {
                return;
            }

            TimeEntry item = e.Row.DataContext as TimeEntry;

            if (item.HasValues()) {
                return;
            }

            //ItemsSource.Remove(item);
        }
    }
}
