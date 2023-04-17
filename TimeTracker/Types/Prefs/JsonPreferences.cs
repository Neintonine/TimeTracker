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

    public string LastSave { get; set; }
    public string TicketURL { get; set; }

    private string _filePath;

    public JsonPreferences()
    {
        _filePath = Path.GetFullPath(RELATIVE_PATH);
    }

    public void Load()
    {
        JObject origin = JObject.Parse(File.ReadAllText(_filePath));

        LastSave = this.EnsureValue<string>(origin, LAST_SAVE_KEY);
        TicketURL = this.EnsureValue<string>(origin, TICKET_URL_KEY);
    }

    public void Save()
    {
        
    }

    private T EnsureValue<T>(
        JObject origin,
        string key,
        T defaultValue = default(T)
    )
    {
        if (!origin.ContainsKey(key))
        {
            return defaultValue;
        }

        return origin[key].Value<T>();
    }
}