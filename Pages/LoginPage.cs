using OpenQA.Selenium;

namespace SauceDemoTest.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        public LoginPage(IWebDriver driver) => this.driver = driver;

        private IWebElement Username => driver.FindElement(By.Id("user-name"));
        private IWebElement Password => driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => driver.FindElement(By.Id("login-button"));

        public void Login(string username, string password)
        {
            Username.SendKeys(username);
            Password.SendKeys(password);
            LoginButton.Click();
        }
    }
}
