namespace Sfa.SpecBind.Enhancements.Hooks
{
    using BoDi;

    using Sfa.SpecBind.Enhancements.Selenium;
    using Sfa.SpecBind.Enhancements.Services;

    public class IoC
    {
        public static IObjectContainer Initialise(IObjectContainer objectContainer)
        {
            objectContainer.RegisterTypeAs<BrowserStackApi, IBrowserStackApi>();
            objectContainer.RegisterTypeAs<WebRequestRetryService, IRetryWebRequests>();
            objectContainer.RegisterTypeAs<ConsoleLogger, ILog>();
            return objectContainer;
        }
    }
}