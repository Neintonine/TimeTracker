using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;
using TimeTracker.Types;
using Path = System.IO.Path;

namespace TimeTracker.Controls.Modals
{
    /// <summary>
    /// Interaction logic for ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : UserControl
    {
        private const string MISSING_FILE_RESPONSE = "File does not exist.";
        private const string MISSING_FILE_PERMISSIONS_RESPONSE = "Can't read file. Possibly missing permissions or the file is used.";
        private const string FORMAT_NOT_SUPPORTED_RESPONSE = "Format is not supported.";

        private const string CORRECT_RESPONSE = "All ready";

        private static Brush CORRECT_BRUSH = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        private static Brush FAILED_BRUSH = new SolidColorBrush(Color.FromRgb(255, 0, 0));

        private static string[] ALLOWED_EXTENSIONS = new[]
        {
            "xlam", "xlsx", "xltx", "xlsm", "xltm"
        };

        private FileHandler _fileHandler;

        public ImportDialog(FileHandler file)
        {
            _fileHandler = file;

            InitializeComponent();
            FileSelect.PathInput.TextChanged += PathChanged;
        }

        private void PathChanged(object sender, TextChangedEventArgs e)
        {
            string path = FileSelect.PathInput.Text;

            if (!File.Exists(path))
            {
                SetPathResponse(MISSING_FILE_RESPONSE, false);
                return;
            }

            if (!ALLOWED_EXTENSIONS.Contains(Path.GetExtension(path).TrimStart('.')))
            {
                SetPathResponse(FORMAT_NOT_SUPPORTED_RESPONSE, false);
                return;
            }

            bool canRead = false;
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    canRead = fs.CanRead;
                }
            }
            catch
            {
                canRead = false;
            }

            if (!canRead)
            {
                SetPathResponse(MISSING_FILE_PERMISSIONS_RESPONSE, false);
                return;
            }

            SetPathResponse("", true);
        }

        private void SetPathResponse(string response, bool correct)
        {
            PathResponse.Content = response;
            PathResponse.Foreground = correct ? CORRECT_BRUSH : FAILED_BRUSH;

            ContinueButton.IsEnabled = correct;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }

        private void CloseDialog()
        {
            DialogHost.Close(null);
        }

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.Close(null);

            ImportInterpretationDialog dialog = new ImportInterpretationDialog(_fileHandler);

            LoadingModal modal = LoadingModal.Display(null, "Interpret File...");

            await dialog.LoadSpreadsheetData(FileSelect.PathInput.Text);

            modal.Remove();

            DialogHost.Show(dialog);
        }
    }
}
