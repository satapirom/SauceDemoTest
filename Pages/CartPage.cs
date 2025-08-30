using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace SauceDemoTest.Pages
{
    public class CartPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void RemoveProduct(string productName)
        {
            var removeBtn = driver.FindElements(By.XPath($"//div[contains(text(),'{productName}')]/following-sibling::div/button")).FirstOrDefault();
            if (removeBtn != null)
                removeBtn.Click();
        }

        public void Checkout()
        {
            // ใช้ class selector แทน id
            var checkoutBtn = wait.Until(d => d.FindElement(By.ClassName("checkout_button")));
            checkoutBtn.Click();
        }
    }
}

