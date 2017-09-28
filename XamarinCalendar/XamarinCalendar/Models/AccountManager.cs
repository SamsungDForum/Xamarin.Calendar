using System;
using System.Threading;
using System.Threading.Tasks;

using XamarinCalendar.Services;

using Tizen.Applications;

namespace XamarinCalendar.Models
{
    public class AccountManager
    {        
        public GoogleCodeData codeData { get; private set; }
        public GoogleTokenData tokenData { get; private set; }
        public bool isLoggedIn { get; private set; }
       
        AuthService authService;
 
        class TokenTimerData
        {
            public Timer timer;
            public WaitHandle waitHandle;
        }

        public AccountManager(AuthService service)
        {
            authService = service;

            isLoggedIn = false;
        }

        public void LoadInfo()
        {
            tokenData = new GoogleTokenData();

            if (!Preference.Contains("access_token") || !Preference.Contains("refresh_token") ||
                !Preference.Contains("issued_date") || !Preference.Contains("expires"))
            {
                isLoggedIn = false;
                return;
            }

            tokenData.access_token = Preference.Get<string>("access_token");
            tokenData.refresh_token = Preference.Get<string>("refresh_token");
            tokenData.issued_date = DateTime.ParseExact(Preference.Get<string>("issued_date"), "yyyy-MM-dd HH:mm:ss", null);
            tokenData.expires_in = Preference.Get<int>("expires");

            isLoggedIn = true;
        }

        public async Task Prepare()
        {
            codeData = await authService.RequestCode();
        }

        public async Task RequestAccount()
        {
            TokenTimerData d = new TokenTimerData();

            TimerCallback timerCallback = new TimerCallback(TokenTimerCb);

            Timer timer = new Timer(timerCallback, d, 5000, 5000);

            d.timer = timer;
            d.waitHandle = new AutoResetEvent(false);

            await Task.Run(() => d.waitHandle.WaitOne());
        }

        public async void TokenTimerCb(object data)
        {
            TokenTimerData d = (TokenTimerData)data;

            tokenData = await authService.RequestToken(codeData.device_code);

            if (tokenData != null)
            {
                tokenData.issued_date = DateTime.Now;

                Preference.Set("access_token", tokenData.access_token);
                Preference.Set("refresh_token", tokenData.refresh_token);
                Preference.Set("issued_date", tokenData.issued_date.ToString("yyyy-MM-dd HH:mm:ss"));
                Preference.Set("expires", tokenData.expires_in);
                
                d.timer.Dispose(d.waitHandle);
                d.timer = null;
            }
        }

        public void RevokeAccount()
        {
            authService.RevokeToken(tokenData.refresh_token);

            Preference.RemoveAll();
        }
    }
}
