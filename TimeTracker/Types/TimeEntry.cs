#nullable enable
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TimeTracker.Types
{
    public partial class TimeEntry : INotifyPropertyChanged
    {
        public enum Status
        {
            NoStatus,
            DontBook,
            Booked,
        }

        private DateTime _date;
        private DateTime? _from;
        private DateTime? _to;
        private Status _status;
        private string? _project = null;
        private string? _action = null;


        public TimeEntry()
        {
            _date = DateTime.Today;
            
            _status = Status.NoStatus;

        }
        
        public bool HasValues()
        {
            return 
                _from != null || 
                _to != null || 
                _status != Status.NoStatus || 
                _project != null || 
                _action != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}