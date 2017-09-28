using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using Google.Apis.Calendar.v3.Data;

namespace XamarinCalendar.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarItem : RelativeLayout
    {
        public enum ItemType
        {
            White,
            Red,
            Blue,
            Green,
        }

        public ItemType Type;

        public CalendarItem()
        {
            InitializeComponent();

            Type = ItemType.White;
        }

        public void SetDate(string d, int day)
        {
            date.Text = d;

            if (day == -1)
                Type = ItemType.Green;
            else if (day == 0)
                Type = ItemType.Red;
            else if (day == 6)
                Type = ItemType.Blue;

            SetTextColor();
        }

        public void SetEvent(Event e)
        {
            string str;

            if (e.Start.DateTime != null)
            {
                str = String.Format("{0:HH:mm} ", e.Start.DateTime) + e.Summary;
            }
            else
            {
                str = e.Summary;
            }

            eventList.Children.Add(new Label {
                Text = str,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.Gray,
                FontSize = 45,
                LineBreakMode = LineBreakMode.TailTruncation,
            });
        }

        private void SetTextColor()
        {
            if (Type == ItemType.White)
                date.TextColor = Color.White;
            else if (Type == ItemType.Red)
                date.TextColor = Color.IndianRed;
            else if (Type == ItemType.Blue)
                date.TextColor = Color.CornflowerBlue;
            else if (Type == ItemType.Green)
                date.TextColor = Color.YellowGreen;
        }
    }
}