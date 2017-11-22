using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

using Tizen;

namespace XamarinCalendar.Services
{
    class CalendarDataService
    {
        // change to your own id and secret
        private readonly string clientId = "710312118591-uhlb0o3tbrjb1hkdgos19chd4e61mu0j.apps.googleusercontent.com";
        private readonly string clientSecret = "urjFxykdcnoYy7mhMfSysiIN";

        CalendarService service;
        string accessToken, refreshToken;
        DateTime issuedDate;
        int expires;

        public CalendarDataService()
        {
            accessToken = App.AppAccountManager.tokenData.access_token;
            refreshToken = App.AppAccountManager.tokenData.refresh_token;
            issuedDate = App.AppAccountManager.tokenData.issued_date;
            expires = App.AppAccountManager.tokenData.expires_in;
        }

        public void RequestService()
        {
            var token = new Google.Apis.Auth.OAuth2.Responses.TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresInSeconds = expires,
                IssuedUtc = issuedDate
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }
            });

            UserCredential credential = new UserCredential(flow, "user", token);

            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "XamarinCalendar",
            });
        }

        public Events GetEvent(DateTime from, DateTime to)
        {
            Events events = null;

            try
            {
                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = from;
                request.TimeMax = to;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                events = request.Execute();
            }
            catch (Exception e)
            {
                Log.Error("CALENDAR", e.Message);
            }

            return events;
        }
    }
}
