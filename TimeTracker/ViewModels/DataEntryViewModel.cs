using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using TimeTracker.Types;

namespace TimeTracker.ViewModels;

public class DataEntryViewModel: SessionViewModelBase
{
    public ObservableCollection<TimeEntry> Entries => SessionHandler.FileHandler.Entries;

    public DataEntryViewModel(SessionHandler sessionHandler) : base(sessionHandler)
    { }

    public override void HandleFileChange()
    {
        OnPropertyChanged(nameof(Entries));
    }
}