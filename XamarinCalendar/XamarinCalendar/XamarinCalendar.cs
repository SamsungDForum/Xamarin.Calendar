using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using XamarinCalendar.Models;
using XamarinCalendar.Services;
using XamarinCalendar.Views;

namespace XamarinCalendar
{
    public class App : Xamarin.Forms.Application
    {
        public static AccountManager AppAccountManager { get; private set; }

        public App()
        {
            AppAccountManager = new AccountManager(new AuthService());

            AppAccountManager.LoadInfo();

            if (AppAccountManager.isLoggedIn)
            {
                MainPage = new CalendarPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
