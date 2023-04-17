using System.Collections.ObjectModel;
using TimeTracker.Types;

namespace TimeTracker;

public class ApplicationContext
{
    public ObservableCollection<TimeEntry> Entries { get; }

    public ApplicationContext()
    {
        Entries = new ObservableCollection<TimeEntry>();
    }
}