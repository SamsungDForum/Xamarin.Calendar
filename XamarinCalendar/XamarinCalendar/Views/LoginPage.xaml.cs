using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginInfoPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoginBtn.Focus();
        }
    }
}