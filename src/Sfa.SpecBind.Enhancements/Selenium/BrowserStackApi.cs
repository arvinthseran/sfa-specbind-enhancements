namespace Sfa.SpecBind.Enhancements.Selenium
{
    using System;
    using System.Net;
    using System.Text;

    using global::SpecBind.BrowserSupport;
    using global::SpecBind.Helpers;

    using OpenQA.Selenium.Remote;

    using Sfa.SpecBind.Enhancements.Services;

    public class BrowserStackApi : IBrowserStackApi
    {
        private readonly IBrowser _browser;

        private readonly IRetryWebRequests _retryService;

        private readonly ILog _logger;

        public BrowserStackApi(IBrowser browser, IRetryWebRequests retryService, ILog logger)
        {
            _browser = browser;
            _retryService = retryService;
            _logger = logger;
        }

        public void FailTestSession(Exception testError)
        {
            try
            {
                const string ReqString = "{\"status\":\"error\", \"reason\":\"\"}";

                var requestData = Encoding.UTF8.GetBytes(ReqString);
                var myUri = new Uri($"https://www.browserstack.com/automate/sessions/{this.FindSessionId()}.json");
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myWebRequest.ContentType = "application/json";
                myWebRequest.Method = "PUT";
                myWebRequest.ContentLength = requestData.Length;
                using (var st = myWebRequest.GetRequestStream())
                {
                    st.Write(requestData, 0, requestData.Length);
                }

                var myCredentialCache = new CredentialCache { { myUri, "Basic", FindNetworkCredential() } };
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Credentials = myCredentialCache;


                _retryService.RetryWeb(() => myWebRequest.GetResponse().Close(), x => _logger.Error(string.Empty, x));

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to connect to browser stack api", ex);
            }
        }

        public string FindSessionId()
        {
            var sessionId = (this._browser.Driver() as RemoteWebDriver)?.SessionId;

            return sessionId?.ToString();
        }

        public NetworkCredential FindNetworkCredential()
        {
            var settings = SettingHelper.GetConfigurationSection().BrowserFactory.Settings;
            return new NetworkCredential(settings["browserstack.user"].Value, settings["browserstack.key"].Value);
        }
    }
}