namespace Sfa.SpecBind.Enhancements.Selenium
{
    using global::SpecBind.BrowserSupport;

    using OpenQA.Selenium;

    public static class BrowserExtensions
    {
        public static IWebDriver Driver(this IBrowser browser)
        {
            var propertyInfo = browser.GetType().GetProperty("Driver");
            var getter = propertyInfo.GetGetMethod();
            return getter.Invoke(browser, new object[] { }) as IWebDriver;
        }
    }
}