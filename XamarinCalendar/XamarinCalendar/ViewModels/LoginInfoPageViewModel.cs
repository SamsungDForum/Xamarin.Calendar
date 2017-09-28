using System;

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using XamarinCalendar.Models;

namespace XamarinCalendar.ViewModels
{
    class LoginInfoPageViewModel : INotifyPropertyChanged
    {
        GoogleCodeData codeData;

        public delegate void RequestAccountDoneEvent(object sender, EventArgs args);
        public event RequestAccountDoneEvent RequestAccountDone;

        public event PropertyChangedEventHandler PropertyChanged;

        public GoogleCodeData CodeData
        {
            set
            {
                if (codeData != value)
                {
                    codeData = value;
                    OnPropertyChanged("CodeData");
                }
            }
            get
            {
                return codeData;
            }
        }

        public LoginInfoPageViewModel()
        {
            RequestCodeData();
        }

        private async void RequestCodeData()
        {
            await App.AppAccountManager.Prepare();
            CodeData = App.AppAccountManager.codeData;

            RequestTokenData();
        }

        private async void RequestTokenData()
        {
            await App.AppAccountManager.RequestAccount();
            OnRequestDone();
        }

        protected void OnRequestDone()
        {
            RequestAccountDone?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
