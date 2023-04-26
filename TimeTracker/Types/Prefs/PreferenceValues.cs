using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeTracker.Types.Prefs;

public class PreferenceValues : INotifyPropertyChanged
{
    private string _ticketUrl;
    private string _lastSave;

    public string LastSave
    {
        get =>  _lastSave;
        set
        {
            _lastSave = value;
            OnPropertyChanged();
        }
    }

    public string TicketURL
    {
        get => _ticketUrl;
        set { _ticketUrl = value; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}