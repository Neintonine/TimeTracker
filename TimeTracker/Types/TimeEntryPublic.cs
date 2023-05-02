using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Policy;

namespace TimeTracker.Types
{
    public partial class TimeEntry
    {
        public FileHandler File { private get; set; }

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public int CalenderWeek =>
            new CultureInfo("de-de").Calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        public string From
        {
            get { return ConvertTimeToString(_from); }
            set {
                _from = ConvertStringToTime(value);
                NotifyPropertyChanged("Duration");
            }
        }

        public string To
        {
            get { return ConvertTimeToString(_to); }
            set
            {
                _to = ConvertStringToTime(value);
                NotifyPropertyChanged("Duration");
            }
        }

        public string Duration
        {
            get
            {
                if (_from == null || _to == null)
                {
                    return "";
                }

                TimeSpan span = _to.Value - _from.Value;
                return span.ToString(@"hh\:mm") + " (" + span.TotalHours.ToString("F") + ")";
            }
        }

        public Status BookingStatus
        {
            get => _status; 
            set {
                _status = value; 
                NotifyPropertyChanged("BookingStatus");
            }
        }

        public bool ProjectIsSet => !string.IsNullOrEmpty(_project);
        public bool ActionIsSet => !string.IsNullOrEmpty(_action);

        public float ProjectVisualOpacity => ProjectIsSet ? 1f : 0.5f;
        public float ActionVisualOpacity => ActionIsSet ? 1f : 0.5f;


        public string ActionEdit
        {
            get => _action ?? "";
            set => _action = value;
        }

        public string ActionVisual => ActionIsSet ? _action : "[No action set]";
        public string[] ActionRecommendation => File.GetActions(ProjectEdit);

        public string ProjectEdit
        {

            get => _project ?? "";
            set => _project = value;
        }

        public string ProjectVisual => ProjectIsSet ? _project : "[No project set]";
        public string[] ProjectRecommendation => File.GetProjects();

        private string ConvertTimeToString(DateTime? time)
        {
            if (time == null)
            {
                return "";
            }

            return time.Value.ToString("HH:mm");
        }

        private DateTime ConvertStringToTime(string value) 
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("de-DE");

            DateTime result;
            if (DateTime.TryParseExact(value, "H:m", culture, DateTimeStyles.None, out result))
            {
                return result;
            };

            if (DateTime.TryParseExact(value, "H:", culture, DateTimeStyles.None, out result))
            {
                return result;
            };

            if (int.TryParse(value, out int hour))
            {
                result = DateTime.Today;
                result.AddHours(hour);
            }

            return DateTime.MinValue;
        }
    }
}