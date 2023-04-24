using System.Collections.ObjectModel;

namespace TimeTracker.Types;

public class FileHandler
{
    private ObservableCollection<TimeEntry> _entries;

    public ObservableCollection<TimeEntry> Entries => _entries;
    public string Path { get; private set; }

    public bool IsNew { get; private set; }

    private FileHandler(string path = null)
    {
        _entries = new ObservableCollection<TimeEntry>();

        IsNew = path == null;

        Path = path;
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
}