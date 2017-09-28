using System.Collections.Generic;

using Google.Apis.Calendar.v3.Data;

namespace XamarinCalendar.Models
{
    class CalendarItemData
    {
        public List<Event> EventList { get; set; }
        public int Index;

        public CalendarItemData(int n)
        {
            EventList = new List<Event>();
            Index = n;
        }
    }
}
