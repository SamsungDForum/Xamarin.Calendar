using System;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XamarinCalendar.Models;
using Tizen;

namespace XamarinCalendar.Services
{
    public class AuthService
    {
        // change to your own id and secret
        private readonly string clientId = "710312118591-uhlb0o3tbrjb1hkdgos19chd4e61mu0j.apps.googleusercontent.com";
        private readonly string clientSecret = "urjFxykdcnoYy7mhMfSysiIN";

        private readonly string scope = "https://www.googleapis.com/auth/calendar.readonly";

        GoogleCodeData codeData;
        GoogleTokenData tokenData;

        public AuthService()
        {

        }

        public async Task<GoogleCodeData> RequestCode()
        {
            var requestUrl =
               "https://accounts.google.com/o/oauth2/device/code"
               + "?client_id=" + clientId
               + "&scope=" + scope;

            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.PostAsync(requestUrl, null);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                codeData = JsonConvert.DeserializeObject<GoogleCodeData>(json);
            }
            catch (HttpRequestException e)
            {
                Log.Error("CALENDAR", e.Message);
                return null;
            }

            return codeData;
        }

        public async Task<GoogleTokenData> RequestToken(string deviceCode)
        {
            var requestUrl =
                   "https://www.googleapis.com/oauth2/v4/token"
                   + "?client_id=" + clientId
                   + "&client_secret=" + clientSecret
                   + "&code=" + deviceCode
                   + "&grant_type=http://oauth.net/grant_type/device/1.0";

            tokenData = null;

            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.PostAsync(requestUrl, null);

                if (response.StatusCode.ToString().Equals("OK"))
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tokenData = JsonConvert.DeserializeObject<GoogleTokenData>(json);
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error("CALENDAR", e.Message);
            }

            return tokenData;
        }

        public void RevokeToken(string refreshToken)
        {
            var requestUrl =
                "https://accounts.google.com/o/oauth2/revoke"
                + "?token=" + refreshToken;

            try
            {
                var httpClient = new HttpClient();
            }
            catch (HttpRequestException e)
            {
                Log.Error("CALENDAR", e.Message);
            }
        }
    }
}
