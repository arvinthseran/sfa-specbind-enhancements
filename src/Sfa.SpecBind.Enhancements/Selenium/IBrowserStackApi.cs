namespace Sfa.SpecBind.Enhancements.Selenium
{
    using System;

    public interface IBrowserStackApi
    {
        void FailTestSession(Exception testError);
    }
}