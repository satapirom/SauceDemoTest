using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SauceDemoTest.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        // Constructor
        public LoginPage(IWebDriver webDriver)
        {
            this.driver = webDriver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        // Locators
        private By usernameField = By.Id("user-name");
        private By passwordField = By.Id("password");
        private By loginButton = By.Id("login-button");

        // Methods
        public void EnterUsername(string username)
        {
            driver.FindElement(usernameField).SendKeys(username);
        }
        public void EnterPassword(string password)
        {
            driver.FindElement(passwordField).SendKeys(password);
        }
        public void ClickLoginButton()
        {
            driver.FindElement(loginButton).Click();
        }
        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLoginButton();
        }
    }
}
