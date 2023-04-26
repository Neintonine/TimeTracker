using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using SQLite;

namespace TimeTracker.Types;

public class FileHandler
{
    private ObservableCollection<TimeEntry> _entries;
    private SQLiteAsyncConnection _connection;


    public ObservableCollection<TimeEntry> Entries => _entries;
    public string Path { get; private set; }

    public bool IsNew { get; private set; }

    private FileHandler(ObservableCollection<TimeEntry> entries)
    {
        _entries = entries;

        _entries.CollectionChanged += HandleListChange;
        foreach (TimeEntry timeEntry in entries)
        {
            timeEntry.PropertyChanged += HandlePropertyChange;
        }
    }

    private async void HandlePropertyChange(object sender, PropertyChangedEventArgs e)
    {
        TimeEntry entry = (TimeEntry)sender;
        if (_connection == null)
        {
            return;
        }

        await _connection.UpdateAsync(entry);
    }

    private async void HandleListChange(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (_connection == null)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                await _connection.InsertAllAsync(e.NewItems);
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (object item in e.OldItems)
                {
                    await _connection.DeleteAsync(item);
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                await _connection.DeleteAllAsync<TimeEntry>();
                break;
            default:
                break;
        }
    }

    public async Task Save(string path)
    {
        if (!IsNew)
        {
            return;
        }

        _connection = new SQLiteAsyncConnection(path);
        await _connection.CreateTableAsync<TimeEntry>();

        await _connection.DeleteAllAsync<TimeEntry>();

        foreach (TimeEntry timeEntry in _entries)
        {
            timeEntry.PropertyChanged += HandlePropertyChange;
        }

        await _connection.InsertAllAsync(_entries);

        Path = path;
        IsNew = false;

        _entries.CollectionChanged += HandleListChange;
    }

    public static FileHandler Create()
    {
        return new FileHandler(new ObservableCollection<TimeEntry>())
        {
            IsNew = true,
            Path = ""
        };
    }

    public static async Task<FileHandler> Load(string path)
    {
        SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path);

        List<TimeEntry> entryList = new List<TimeEntry>();

        ObservableCollection<TimeEntry> entries =
            new ObservableCollection<TimeEntry>(await connection.Table<TimeEntry>().ToArrayAsync());
        

        return new FileHandler(entries)
        {
            _connection = connection,
            Path = path,
            IsNew = true
        };
    }
}