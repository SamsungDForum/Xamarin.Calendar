using System;

namespace XamarinCalendar.Models
{
    public class GoogleTokenData
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime issued_date { get; set; }
        public int expires_in { get; set; }
    }
}
