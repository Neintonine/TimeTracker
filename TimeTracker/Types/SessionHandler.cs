using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TimeTracker.Types.Prefs;
using TimeTracker.ViewModels;

namespace TimeTracker.Types;

public class SessionHandler: INotifyPropertyChanged
{
    private List<SessionViewModelBase> _registeredViewModels = new();
    private IPreferenceHandler _preferences;

    private FileHandler _fileHandler;

    public SessionHandler(IPreferenceHandler preferences)
    {
        _preferences = preferences;
    }

    public FileHandler FileHandler
    {
        get => _fileHandler;
        set
        {
            _fileHandler = value;
            foreach (SessionViewModelBase viewModel in _registeredViewModels)
            {
                viewModel.HandleFileChange();
            }
        }
    }

    public IPreferenceHandler Preferences => _preferences;

    public event PropertyChangedEventHandler PropertyChanged;

    public void Register(SessionViewModelBase viewModel)
    {
        _registeredViewModels.Add(viewModel);
    }

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