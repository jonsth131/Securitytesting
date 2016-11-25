using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Securitytesting.Pages;

namespace Securitytesting.Factories
{
    public static class PageObjectFactory
    {
        public static T GetInitializedPageObject<T>(IWebDriver driver) where T : PageObject
        {
            var pageObject = (T)Activator.CreateInstance(typeof(T), driver);
            PageFactory.InitElements(driver, pageObject);
            return pageObject;
        }
    }
}
