using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinCalendar.Models;
using XamarinCalendar.ViewModels;

namespace XamarinCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginInfoPage : ContentPage
    {
        public static readonly BindableProperty CodeDataProperty = BindableProperty.Create("CodeData", typeof(GoogleCodeData), typeof(LoginInfoPage), default(GoogleCodeData));
        public GoogleCodeData CodeData
        {
            get
            {
                return (GoogleCodeData)GetValue(CodeDataProperty);
            }
            set
            {
                SetValue(CodeDataProperty, value);
            }
        }

        public LoginInfoPage()
        {
            InitializeComponent();

            ((LoginInfoPageViewModel)BindingContext).RequestAccountDone += RequestAccountDone;
            PropertyChanged += CodeDataChanged;
        }

        private void CodeDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CodeData"))
            {
                UpdateCodeInfo();
            }
        }

        void RequestAccountDone(object sender, EventArgs e)
        {
            LaunchCalendar();
        }

        private void UpdateCodeInfo()
        {
            verificationUrl.Text = CodeData.verification_url;
            userCode.Text = CodeData.user_code;
        }

        private void LaunchCalendar()
        {
            App.Current.MainPage = new CalendarPage();
        }

        private void OnBtnClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}