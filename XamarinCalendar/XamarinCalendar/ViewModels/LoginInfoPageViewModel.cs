using System;
using System.ComponentModel;
using XamarinCalendar.Models;

namespace XamarinCalendar.ViewModels
{
    public enum LoginResult
    {
        OK,
        ERR,
    }

    class LoginEventArgs : EventArgs
    {
        public LoginResult Result { get; set; }

        public LoginEventArgs(LoginResult r)
        {
            Result = r;
        }
    }

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

            if (CodeData == null)
                OnRequestDone(LoginResult.ERR);
            else
                RequestTokenData();
        }

        private async void RequestTokenData()
        {
            await App.AppAccountManager.RequestAccount();
            OnRequestDone(LoginResult.OK);
        }

        protected void OnRequestDone(LoginResult Result)
        {
            RequestAccountDone?.Invoke(this, new LoginEventArgs(Result));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
