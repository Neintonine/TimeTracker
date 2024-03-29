﻿using System;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTracker.Types;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for DataEntry.xaml
    /// </summary>
    public partial class DataEntry : UserControl
    {
        public DataEntry()
        {
            InitializeComponent();
            Data.ItemsSource = new ObservableCollection<TimeEntry>();
        }

        public void SetContext(ApplicationContext context)
        {
            Data.ItemsSource = context.Entries;
        }
    }
}
