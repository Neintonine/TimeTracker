using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using TimeTracker.Controls.Modals;
using TimeTracker.Types;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ICommand ImportSpreadsheet { get; private set; }

        public ApplicationContext AppContext { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            AppContext = new ApplicationContext();
            
            DataContext = this;
            ImportSpreadsheet = new ActionCommand(ImportSpreadsheetCommand);

            DataEntry.SetContext(AppContext);
        }

        public void ImportSpreadsheetCommand()
        {
            ImportDialog dialog = new ImportDialog(AppContext);
            DialogHost.Show(dialog);
        }
    }
}
