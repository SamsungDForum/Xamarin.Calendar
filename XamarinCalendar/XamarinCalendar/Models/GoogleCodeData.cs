﻿namespace XamarinCalendar.Models
{
    public class GoogleCodeData
    {
        public string device_code { get; set; }
        public string user_code { get; set; }
        public string verification_url { get; set; }
        public int expires_in { get; set; }
        public int interval { get; set; }        
    }
}
