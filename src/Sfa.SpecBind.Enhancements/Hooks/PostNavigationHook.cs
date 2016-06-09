namespace Sfa.SpecBind.Enhancements.Hooks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using global::SpecBind.Actions;
    using global::SpecBind.BrowserSupport;
    using global::SpecBind.Pages;

    using OpenQA.Selenium;

    using Sfa.SpecBind.Enhancements.Selenium;

    public class TestPostNavigateHook : NavigationPostAction
    {
        private readonly IBrowser _browser;

        public TestPostNavigateHook(IBrowser browser)
        {
            this._browser = browser;
        }

        protected override void OnPageNavigate(IPage page, PageNavigationAction.PageAction actionType, IDictionary<string, string> pageArguments)
        {
            this.CheckForJavascriptErrors();
        }

        public void CheckForJavascriptErrors()
        {
            var errors = ((IJavaScriptExecutor)this._browser.Driver()).ExecuteScript("return window.jsErrors") as ReadOnlyCollection<object> ?? new ReadOnlyCollection<object>(new List<object>());
            if (errors.Any())
            {
                throw new ApplicationException(string.Join("\n", errors.Select(x => x.ToString())));
            }
        }
    }
}
