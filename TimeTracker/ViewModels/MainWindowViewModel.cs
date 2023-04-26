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
    public bool SaveEnabled => SessionHandler.FileHandler.IsNew;
    public string SaveText => "_Save" + (SessionHandler.FileHandler.IsNew ? "" : " - Auto saved");

    public ICommand ImportSpreadsheet { get; private set; }
    public ICommand CreateNewFile{ get; private set; }
    public ICommand SaveFile { get; private set; }
    public ICommand LoadFile { get; private set; }
    
    public MainWindowViewModel(SessionHandler session): base(session)
    {
        if (!string.IsNullOrEmpty(session.Preferences.Values.LastSave))
        {
            LoadingFile(session.Preferences.Values.LastSave, "Loading previous file...");
        }

        CreateNewFile = new ActionCommand(CreateNewFileCommand);
        SaveFile = new ActionCommand(SaveFileCommand);
        LoadFile = new ActionCommand(LoadFileCommand);

        ImportSpreadsheet = new ActionCommand(ImportSpreadsheetCommand);
    }

    private void LoadFileCommand()
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            AddExtension = true,
            DefaultExt = ".sav",
            Filter = "Time Tracker file (.sav)|*.sav"
        };

        bool? result = dialog.ShowDialog();
        if (result != true)
        {
            return;
        }

        LoadingFile(dialog.FileName);
    }

    private async void LoadingFile(string path, string loadingTitle = "Loading...")
    {
        SessionHandler.Preferences.Values.LastSave = path;

        LoadingModal modal = LoadingModal.Display(loadingTitle);
        SessionHandler.FileHandler = await FileHandler.Load(path);
        modal.Remove();
    }

    private void CreateNewFileCommand()
    {
        SessionHandler.FileHandler = FileHandler.Create();
    }

    public async void SaveFileCommand()
    {
        SaveFileDialog dialog = new SaveFileDialog
        {
            CheckPathExists = true,
            AddExtension = true,
            OverwritePrompt = true,
            DefaultExt = ".sav"
        };

        bool? result = dialog.ShowDialog();

        if (result != true)
        {
            return;
        }

        await SessionHandler.FileHandler.Save(dialog.FileName);
        SessionHandler.Preferences.Values.LastSave = dialog.FileName;

        OnPropertyChanged(nameof(FilePath));
        OnPropertyChanged(nameof(SaveText));
        OnPropertyChanged(nameof(SaveEnabled));
    }

    public override void HandleFileChange()
    {
        OnPropertyChanged(nameof(FilePath));
        OnPropertyChanged(nameof(SaveText));
        OnPropertyChanged(nameof(SaveEnabled));
    }

    public void ImportSpreadsheetCommand()
    {
        ImportDialog dialog = new ImportDialog(SessionHandler.FileHandler);
        DialogHost.Show(dialog);
    }
}