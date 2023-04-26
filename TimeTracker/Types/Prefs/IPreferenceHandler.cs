using DocumentFormat.OpenXml.Wordprocessing;

namespace TimeTracker.Types.Prefs;

public interface IPreferenceHandler
{
    bool CanLoad { get; }
    PreferenceValues Values { get; }


    void Load();
    void Save();
}