using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SQLite;

namespace TimeTracker.Types;

public class FileHandler
{
    private ObservableCollection<TimeEntry> _entries;
    private SQLiteAsyncConnection _connection;


    public ObservableCollection<TimeEntry> Entries => _entries;
    public string Path { get; private set; }

    public bool IsNew { get; private set; }

    private FileHandler()
    {
    }

    public async Task Save(string path)
    {
        if (!IsNew)
        {
            return;
        }

        _connection = new SQLiteAsyncConnection(path);
        await _connection.CreateTableAsync<TimeEntry>();

        List<Task<int>> inserts = new List<Task<int>>();
        foreach (TimeEntry timeEntry in _entries)
        {
            inserts.Add(_connection.InsertAsync(timeEntry));
        }

        await Task.WhenAll(inserts);

        Path = path;
        IsNew = false;
    }

    public static FileHandler Create()
    {
        return new FileHandler()
        {
            _entries = new ObservableCollection<TimeEntry>(),
            IsNew = true,
            Path = ""
        };
    }

    public static async Task<FileHandler> Load(string path)
    {
        SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path);
        ObservableCollection<TimeEntry> entries =
            new ObservableCollection<TimeEntry>(await connection.Table<TimeEntry>().ToArrayAsync());
        return new FileHandler()
        {
            _connection = connection,
            _entries = entries,
            Path = path,
            IsNew = true
        };
    }
}