using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Google.Apis.Calendar.v3.Data;

using Xamarin.Forms;
   
using XamarinCalendar.Models;
using XamarinCalendar.Services;

namespace XamarinCalendar.ViewModels
{
    class CalendarPageViewModel : INotifyPropertyChanged
    {
        private CalendarDataService _calendarDataService;

        public List<CalendarItemData> ItemList { get; set; }

        public DateTime currentDate;
        private ICommand _refreshCommand, _prevCommand, _nextCommand; 

        public DateTime CurrentDate
        {
            set
            {
                currentDate = value;
                CreateCalendarData();
            }
            get
            {
                return currentDate;
            }
        }

        public ICommand RefreshBtnCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new Command(() => CurrentDate = CurrentDate.AddMilliseconds(1));
                }
                return _refreshCommand;
            }
        }

        public ICommand PrevBtnCommand
        {
            get
            {
                if (_prevCommand == null)
                {
                    _prevCommand = new Command(() => CurrentDate = CurrentDate.AddMonths(-1));
                }
                return _prevCommand;
            }
        }

        public ICommand NextBtnCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new Command(() => CurrentDate = CurrentDate.AddMonths(1));
                }
                return _nextCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarPageViewModel()
        {
            ItemList = new List<CalendarItemData>();

            _calendarDataService = new CalendarDataService();
            _calendarDataService.RequestService();

            CurrentDate = DateTime.Today;
        }

        public void CreateCalendarData()
        {
            CalendarItemData data;
            DateTime firstDay = CurrentDate.AddDays(1 - CurrentDate.Day);
            Events events;

            ItemList.Clear();

            events = _calendarDataService.GetEvent(firstDay, firstDay.AddDays(DateTime.DaysInMonth(firstDay.Year, firstDay.Month)));

            for (int i = 1; i <= DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month); i++)
            {
                data = new CalendarItemData(i);
                ItemList.Add(data);
            }

            foreach (Event e in events.Items)
            {
                if (e.Start.DateTime != null)
                {
                    int index = ((DateTime)e.Start.DateTime).Day;

                    data = ItemList.Find(x => x.Index == index);
                    if (data != null)
                        data.EventList.Add(e);
                }
                else
                {
                    DateTime start = Convert.ToDateTime(e.Start.Date);
                    DateTime end = Convert.ToDateTime(e.End.Date);
                    int d_s = 1;
                    int d_e = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month) + 1;

                    if (start.Year == CurrentDate.Year && start.Month == CurrentDate.Month)
                        d_s = start.Day;

                    if (end.Year == CurrentDate.Year && end.Month == CurrentDate.Month)
                        d_e = end.Day;

                    for (int i = d_s; i < d_e; i++)
                    {
                        data = ItemList.Find(x => x.Index == i);
                        if (data != null)
                            data.EventList.Add(e);
                    }
                }
            }

            OnPropertyChanged("CurrentDate");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
