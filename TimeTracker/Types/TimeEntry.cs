#nullable enable
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using SQLite;

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

        [PrimaryKey, AutoIncrement]
        public int ID {get; set; }

        public ICommand SetStatusBooked { get; private set; }
        public ICommand SetStatusDontBook { get; private set; }

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

            SetStatusBooked = new ActionCommand(() =>
            {
                BookingStatus = Status.Booked;
            });
            SetStatusDontBook = new ActionCommand(() =>
            {
                BookingStatus = Status.DontBook;
            });
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