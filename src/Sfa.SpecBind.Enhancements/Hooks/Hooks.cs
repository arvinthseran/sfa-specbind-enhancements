namespace Sfa.SpecBind.Enhancements.Hooks
{
    using System;
    using System.Linq;

    using BoDi;

    using global::SpecBind.BrowserSupport;
    using global::SpecBind.Helpers;

    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Remote;

    using Sfa.SpecBind.Enhancements.Selenium;
    using Sfa.SpecBind.Enhancements.Services;

    using TechTalk.SpecFlow;

    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            this._objectContainer = IoC.Initialise(objectContainer);
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            Console.WriteLine($"#####################  Feature: {FeatureContext.Current.FeatureInfo.Title}  ######################");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine("##### Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
            
            var log = this._objectContainer.Resolve<ILog>();
            var settings = SettingHelper.GetConfigurationSection().BrowserFactory.Settings;
            foreach (var key in settings.AllKeys.Where(x => !x.StartsWith("browserstack.")))
            {
                if (!string.IsNullOrEmpty(settings[key]?.Value))
                {
                    log.Info($"{key}:\"{settings[key]?.Value}\"");
                }
            }
        }

        [AfterScenario(Order = -10)]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                var driver = this._objectContainer.Resolve<IBrowser>().Driver();
                var settings = SettingHelper.GetConfigurationSection().BrowserFactory.Settings;
                if(driver is RemoteWebDriver && !(driver is PhantomJSDriver) && !string.IsNullOrEmpty(settings["browserstack.user"].Value) && !string.IsNullOrEmpty(settings["browserstack.key"].Value))
                {
                    this._objectContainer.Resolve<IBrowserStackApi>().FailTestSession(ScenarioContext.Current.TestError);
                }
            }
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }
    }
}