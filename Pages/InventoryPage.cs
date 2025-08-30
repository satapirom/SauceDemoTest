using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace SauceDemoTest.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public InventoryPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement CartLink => driver.FindElement(By.ClassName("shopping_cart_link"));

        // เปลี่ยน AddProduct ให้ return bool เพื่อบอกว่าเจอหรือไม่
        public bool AddProduct(string productKeyword)
        {
            var productItem = driver.FindElements(By.ClassName("inventory_item"))
                                    .FirstOrDefault(p => p.FindElement(By.ClassName("inventory_item_name")).Text.Contains(productKeyword));

            if (productItem != null)
            {
                var addBtn = productItem.FindElement(By.TagName("button")); // ปุ่ม Add to cart
                addBtn.Click();
                return true;
            }
            return false;
        }

        public void GoToCart() => CartLink.Click();
    }
}
