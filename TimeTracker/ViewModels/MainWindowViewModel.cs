using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Windows.Input;
using TimeTracker.Controls.Modals;
using TimeTracker.Types;

namespace TimeTracker.ViewModels;

public class MainWindowViewModel: SessionViewModelBase
{

    public string FilePath => SessionHandler.FileHandler.Path;

    public ICommand ImportSpreadsheet { get; private set; }
    public ICommand CreateNewFileCommand { get; private set; }
    public ICommand SaveSameFileCommand { get; private set; }

    public MainWindowViewModel(SessionHandler session): base(session)
    {
        ImportSpreadsheet = new ActionCommand(ImportSpreadsheetCommand);
    }

    public void ImportSpreadsheetCommand()
    {
        ImportDialog dialog = new ImportDialog(SessionHandler.FileHandler);
        DialogHost.Show(dialog);
    }
}