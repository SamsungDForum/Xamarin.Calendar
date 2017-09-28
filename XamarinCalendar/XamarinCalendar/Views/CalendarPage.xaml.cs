using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Google.Apis.Calendar.v3.Data;

using XamarinCalendar.Controls;
using XamarinCalendar.Models;
using XamarinCalendar.ViewModels;

using Tizen.Applications;
using Tizen.Applications.Notifications;

namespace XamarinCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private Button _focusedBtn;
        int row, col;

        public static readonly BindableProperty CurrentDateProperty = BindableProperty.Create("CurrentDate", typeof(DateTime), typeof(CalendarPage), default(DateTime));
        public DateTime CurrentDate
        {
            get
            {
                return (DateTime)GetValue(CurrentDateProperty);
            }
            set
            {
                SetValue(CurrentDateProperty, value);
            }
        }

        public CalendarPage()
        {
            PropertyChanged += ContentChanged;

            InitializeComponent();
        }
        
        public void DrawTopArea()
        {
            DateTime firstDay = CurrentDate.AddDays(1 - CurrentDate.Day);

            calendarGrid.Children.Add(TopArea, 0, 0);
            Grid.SetColumnSpan(TopArea, 7);

            CurMonth.Text = String.Format("{0:D4}. {1:D2}", CurrentDate.Year, CurrentDate.Month);

            col = (int)firstDay.DayOfWeek;
            row = 1;

            for (int i = 0; i < 7; i++)
            {
                string text = firstDay.AddDays(i).DayOfWeek.ToString().Substring(0, 3).ToUpper();

                Label label = new Label
                {
                    Text = text,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                };

                if (text.Equals("SAT"))
                    label.TextColor = Color.CornflowerBlue;
                else if (text.Equals("SUN"))
                    label.TextColor = Color.IndianRed;
                else
                    label.TextColor = Color.White;

                calendarGrid.Children.Add(label, col, row);

                col++;
                if (col == 7)
                {
                    col = 0;
                }
            }
        }

        public void DrawDays()
        {
            DateTime today = DateTime.Today;

            row++;

            int day = 1;
            foreach (CalendarItemData data in ((CalendarPageViewModel)BindingContext).ItemList)
            {
                CalendarItem item = new CalendarItem
                {
                    BindingContext = data
                };

                if (CurrentDate.Year == today.Year && CurrentDate.Month == today.Month && day == today.Day)
                {
                    item.SetDate(day.ToString(), -1);
                    CreateAlarm(data.EventList);
                }
                else
                {
                    item.SetDate(day.ToString(), col);
                }

                for (int i = 0; i < data.EventList.Count; i++)
                {
                    item.SetEvent(data.EventList.ElementAt(i));
                }

                calendarGrid.Children.Add(item, col, row);

                col++;
                if (col == 7)
                {
                    col = 0;
                    row++;
                }
                day++;
            }
        }

        public void DrawCalendar()
        {
            calendarGrid.Children.Clear();

            DrawTopArea();
            DrawDays();

            if (_focusedBtn != null)
                _focusedBtn.Focus();
        }

        private void ContentChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentDate"))
            {
                DrawCalendar();
            }
        }

        private void OnPrevBtnFocused(object sender, EventArgs e)
        {
            _focusedBtn = PrevBtn;
        }

        private void OnNextBtnFocused(object sender, EventArgs e)
        {
            _focusedBtn = NextBtn;
        }

        private void OnRefreshBtnFocused(object sender, EventArgs e)
        {
            _focusedBtn = RefreshBtn;
            RefreshImg.Source = ImageSource.FromFile("btn_calendar_refresh_focus.png");
        }

        private void OnRefreshBtnUnfocused(object sender, EventArgs e)
        {
            RefreshImg.Source = ImageSource.FromFile("btn_calendar_refresh_normal.png");
        }

        private void OnLogoutBtnFocused(object sender, EventArgs e)
        {
            LogoutImg.Source = ImageSource.FromFile("btn_calendar_logout_focus.png");
        }

        private void OnLogoutBtnUnfocused(object sender, EventArgs e)
        {
            LogoutImg.Source = ImageSource.FromFile("btn_calendar_logout_normal.png");
        }

        private void OnLogoutBtnClicked(object sender, EventArgs e)
        {
            App.AppAccountManager.RevokeAccount();

            App.Current.MainPage = new LoginPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NextBtn.Focus();
        }

        public void CreateAlarm(List<Event> eventList)
        {
            AlarmManager.CancelAll();

            foreach (Event e in eventList)
            {
                if (e.Start.DateTime == null)
                    continue;

                Notification n = new Notification
                {
                    Title = e.Summary,
                    IsVisible = true
                };

                n.Content = e.Start.DateTime.Value.ToString("yyyy.MM.dd HH:mm") + " - " + e.End.DateTime.Value.ToString("yyyy.MM.dd HH:mm");

                Notification.ActiveStyle style = new Notification.ActiveStyle();

                style.AddButtonAction(new Notification.ButtonAction { Index = 0, Text = "OK"});

                n.AddStyle(style);

                AlarmManager.CreateAlarm(e.Start.DateTime.Value, n);
            }
        }
    }
}