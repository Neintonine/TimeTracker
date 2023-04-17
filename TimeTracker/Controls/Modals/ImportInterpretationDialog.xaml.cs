using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MaterialDesignThemes.Wpf;
using TimeTracker.Types;

namespace TimeTracker.Controls.Modals
{
    /// <summary>
    /// Interaction logic for ImportInterpretationDialog.xaml
    /// </summary>
    public partial class ImportInterpretationDialog : UserControl, INotifyPropertyChanged
    {
        private readonly ApplicationContext _applicationContext;
        private int _statusColumnIndex = 0;

        public List<string> ColumnNames { get; private set; }
        public int ColumnsCount { get; set; }
        public int RowsCount { get; set; }

        public int StartRow { get; set; } = 1;

        public int DateColumnIndex { get; set; } = 0;
        public int FromColumnIndex { get; set; } = 0;
        public int ToColumnIndex { get; set; } = 0;
        public int ProjectColumnIndex { get; set; } = 0;
        public int ActionColumnIndex { get; set; } = 0;

        public int StatusColumnIndex
        {
            get => _statusColumnIndex;
            set
            {
                _statusColumnIndex = value;
                StatusColumnIndexUpdated();
            }
        }

        public string BookedSymbol { get; set; } = "X";
        public string NotBookedSymbol { get; set; } = "-";

        private void StatusColumnIndexUpdated()
        {
            StatusOptionGrid.IsEnabled = _statusColumnIndex != 0;
        }


        private XLWorkbook _workbook;
        private IXLWorksheet _worksheet;
        private IXLRows _rows;
        private int _lastRow;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImportInterpretationDialog(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            InitializeComponent();
            DataContext = this;
        }

        public async Task LoadSpreadsheetData(string path)
        {
            (_workbook, _worksheet, _rows, _lastRow, List<string> columnNames) = await Task.Run(() =>
            {
                XLWorkbook workbook = new XLWorkbook(path);

                IXLWorksheet _worksheet = workbook.Worksheet(1);

                IXLColumns columns = _worksheet.ColumnsUsed();
                List<string> columnNames = (from column in columns select column.ColumnLetter()).ToList();
                columnNames.Insert(0, "- do not import -");

                IXLRows rows = _worksheet.RowsUsed();
                int lastRow = rows.Last().RangeAddress.FirstAddress.RowNumber;

                return (workbook, _worksheet, rows, lastRow, columnNames);
            });

            ColumnNames = columnNames;
            RowCountDisplay.Text = _rows.Count().ToString();
            NotifyPropertyChanged(nameof(ColumnNames));

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            DialogHost.Close(null);
            _workbook.Dispose();
        }

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.Close(null);

            LoadingModal loading = LoadingModal.Display("Importing...");
            await ImportData();
            loading.Remove();

            _workbook.Dispose();
        }

        private async Task ImportData()
        {
            List<TimeEntry> entries = await Task.Run(() =>
            {
                List<TimeEntry> entries = new List<TimeEntry>();

                bool importDate = DateColumnIndex != 0;
                bool importFrom = FromColumnIndex != 0;
                bool importTo = ToColumnIndex != 0;
                bool importProject = ProjectColumnIndex != 0;
                bool importAction = ActionColumnIndex != 0;
                bool importStatus = StatusColumnIndex != 0;

                if (!importDate &&
                    !importFrom && !importTo &&
                    !importProject && !importAction &&
                    !importStatus)
                {
                    // if no column should be import, then there is no need to start going though the rows.
                    return entries;
                }


                int dateColumnIndex = importDate ? XLHelper.GetColumnNumberFromLetter(ColumnNames[DateColumnIndex]) : 0;
                int fromColumnIndex = importFrom ? XLHelper.GetColumnNumberFromLetter(ColumnNames[FromColumnIndex]) : 0;
                int toColumnIndex = importTo ? XLHelper.GetColumnNumberFromLetter(ColumnNames[ToColumnIndex]) : 0;
                int projectColumnIndex = importProject ? XLHelper.GetColumnNumberFromLetter(ColumnNames[ProjectColumnIndex]) : 0;
                int actionColumnIndex = importAction ? XLHelper.GetColumnNumberFromLetter(ColumnNames[ActionColumnIndex]) : 0;
                int statusColumnIndex = importStatus ? XLHelper.GetColumnNumberFromLetter(ColumnNames[StatusColumnIndex]) : 0;

                for (int i = StartRow; i < _lastRow; i++)
                {
                    IXLRow row = _worksheet.Row(i);

                    try
                    {
                        TimeEntry entry = new TimeEntry();
                        if (importDate)
                        {
                            entry.Date = row.Cell(dateColumnIndex).GetDateTime();
                        }

                        if (importFrom)
                        {
                            entry.From = row.Cell(fromColumnIndex).Value.GetDateTime().ToString("HH:mm");
                        }

                        if (importTo)
                        {
                            entry.To = row.Cell(toColumnIndex).Value.GetDateTime().ToString("HH:mm");
                        }

                        if (importProject)
                        {
                            entry.ProjectEdit = row.Cell(projectColumnIndex).Value.ToString();
                        }

                        if (importAction)
                        {
                            entry.ActionEdit = row.Cell(actionColumnIndex).Value.ToString();
                        }

                        if (importStatus)
                        {
                            string savedStatus = row.Cell(statusColumnIndex).Value.ToString();

                            if (savedStatus == BookedSymbol)
                            {
                                entry.BookingStatus = TimeEntry.Status.Booked;
                            }
                            else if (savedStatus == NotBookedSymbol)
                            {
                                entry.BookingStatus = TimeEntry.Status.DontBook;
                            }
                            else
                            {
                                entry.BookingStatus = TimeEntry.Status.NoStatus;
                            }
                        }

                        entries.Add(entry);
                    }
                    catch (Exception)
                    {
                    }

                }

                return entries;
            });

            foreach (TimeEntry entry in entries)
            {
                _applicationContext.Entries.Add(entry);
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
