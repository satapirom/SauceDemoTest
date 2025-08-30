using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SauceDemoTest.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public CheckoutPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement FirstNameInput => driver.FindElement(By.Id("first-name"));
        private IWebElement LastNameInput => driver.FindElement(By.Id("last-name"));
        private IWebElement ZipInput => driver.FindElement(By.Id("postal-code"));
        private IWebElement ContinueButton => driver.FindElement(By.Id("continue"));
        private IWebElement FinishButton => driver.FindElement(By.Id("finish"));
        private IWebElement CompleteHeader => driver.FindElement(By.ClassName("complete-header"));

        public void FillCheckoutInfo(string firstName, string lastName, string zip)
        {
            FirstNameInput.SendKeys(firstName);
            LastNameInput.SendKeys(lastName);
            ZipInput.SendKeys(zip);
            ContinueButton.Click();
        }

        public void FinishOrder() => FinishButton.Click();

        public string GetCompleteHeader() => CompleteHeader.Text;
    }
}
