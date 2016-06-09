namespace Sfa.SpecBind.Enhancements.Steps
{
    using System;
    using System.Configuration;

    using global::SpecBind.Helpers;

    using Sfa.SpecBind.Enhancements.Services;

    using TechTalk.SpecFlow;

    [Binding]
    public class DataSteps
    {
        private readonly ITokenManager _tokenManager;

        private readonly ILog _log;

        public DataSteps(ITokenManager tokenManager, ILog log)
        {
            this._tokenManager = tokenManager;
            this._log = log;
        }

        [Given(@"I have data in the config")]
        public void GivenIHaveDataInTheConfig(Table data)
        {
            foreach (var row in data.Rows)
            {
                try
                {
                    var value = ConfigurationManager.AppSettings[row["Key"]];
                    this._tokenManager.SetToken(row["Token"], value);
                    this._log.Info("{" + row["Token"] + "} = " + value);
                }
                catch (Exception ex)
                {
                    this._log.Error("problem loading config", ex);
                }
            }
        }

    }
}