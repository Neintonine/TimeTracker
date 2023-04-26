using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DocumentFormat.OpenXml.Drawing.Charts;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using TimeTracker.Controls.Modals;
using TimeTracker.Types;
using TimeTracker.Types.Prefs;
using TimeTracker.ViewModels;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            IPreferenceHandler preferenceHandler = new JsonPreferences();
            if (preferenceHandler.CanLoad)
            {
                preferenceHandler.Load();
            }

            SessionHandler session = new SessionHandler(preferenceHandler)
            {
                FileHandler = FileHandler.Create()
            };

            InitializeComponent();
            DataContext = new MainWindowViewModel(session);
            DataEntry.DataContext = new DataEntryViewModel(session);
        }
    }
}
