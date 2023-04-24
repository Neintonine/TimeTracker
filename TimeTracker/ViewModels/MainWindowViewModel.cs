using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.Win32;
using TimeTracker.Controls.Modals;
using TimeTracker.Types;

namespace TimeTracker.ViewModels;

public class MainWindowViewModel: SessionViewModelBase
{

    public string FilePath => SessionHandler.FileHandler.Path;

    public ICommand ImportSpreadsheet { get; private set; }
    public ICommand CreateNewFile{ get; private set; }
    public ICommand SaveFile { get; private set; }
    public ICommand LoadFile { get; private set; }
    
    public MainWindowViewModel(SessionHandler session): base(session)
    {
        CreateNewFile = new ActionCommand(CreateNewFileCommand);
        SaveFile = new ActionCommand(SaveFileCommand);
        LoadFile = new ActionCommand(LoadFileCommand);

        ImportSpreadsheet = new ActionCommand(ImportSpreadsheetCommand);
    }

    private async void LoadFileCommand()
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.AddExtension = true;
        dialog.DefaultExt = ".sav";
        dialog.Filter = "Time Tracker file (.sav)|*.sav";

        bool? result = dialog.ShowDialog();
        if (result != true)
        {
            return;
        }

        LoadingModal modal = LoadingModal.Display("Loading...");
        SessionHandler.FileHandler = await FileHandler.Load(dialog.FileName);
        modal.Remove();
    }

    private void CreateNewFileCommand()
    {
        SessionHandler.FileHandler = FileHandler.Create();
    }

    public async void SaveFileCommand()
    {
        SaveFileDialog dialog = new SaveFileDialog();

        dialog.CheckPathExists = true;
        dialog.AddExtension = true;
        dialog.OverwritePrompt = true;
        dialog.DefaultExt = ".sav";
        bool? result = dialog.ShowDialog();

        if (result != true)
        {
            return;
        }

        await SessionHandler.FileHandler.Save(dialog.FileName);

        OnPropertyChanged(nameof(FilePath));
    }

    public override void HandleFileChange()
    {
        OnPropertyChanged(nameof(FilePath));
    }

    public void ImportSpreadsheetCommand()
    {
        ImportDialog dialog = new ImportDialog(SessionHandler.FileHandler);
        DialogHost.Show(dialog);
    }
}