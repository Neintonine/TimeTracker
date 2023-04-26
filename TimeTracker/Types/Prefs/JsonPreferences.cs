using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimeTracker.Types.Prefs;

public class JsonPreferences : IPreferenceHandler
{
    private const string RELATIVE_PATH = "prefs.json";

    private const string LAST_SAVE_KEY = "last_save_path";
    private const string TICKET_URL_KEY = "ticket_url";

    bool IPreferenceHandler.CanLoad => File.Exists(_filePath);
    public PreferenceValues Values {get; private set; }

    private string _filePath;

    public JsonPreferences()
    {
        _filePath = Path.GetFullPath(RELATIVE_PATH);
        Values = new PreferenceValues();
        Values.PropertyChanged += ValuesOnPropertyChanged;
    }

    public void Load()
    {
        JObject origin = JObject.Parse(File.ReadAllText(_filePath));

        Values = origin.ToObject<PreferenceValues>();
        Values.PropertyChanged += ValuesOnPropertyChanged;
    }

    private void ValuesOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Save();
    }

    public void Save()
    {
        JObject saveObject = JObject.FromObject(Values);
        File.WriteAllText(_filePath, saveObject.ToString());
    }

    private T EnsureValue<T>(
        JObject origin,
        string key,
        T defaultValue = default
    )
    {
        if (!origin.ContainsKey(key))
        {
            return defaultValue;
        }

        return origin[key].Value<T>();
    }
}