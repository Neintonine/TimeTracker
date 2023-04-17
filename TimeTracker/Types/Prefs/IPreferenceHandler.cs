namespace TimeTracker.Types.Prefs;

public interface IPreferenceHandler
{
    bool CanLoad { get; }

    string LastSave { get; set; }
    string TicketURL { get; set; }
    
    void Load();
    void Save();
}