// Path: SauceDemoTest/Pages/InventoryPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SauceDemoTest.Pages
{
    public class InventoryPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        // Constructor
        public InventoryPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Locators
        private By inventoryContainer = By.Id("inventory_container");
        private By productNames = By.ClassName("inventory_item_name");
        private By addToCartButtons = By.CssSelector(".inventory_item button");
        private By removeButtons = By.CssSelector(".inventory_item button");

        // Methods

        //ค้นหาว่ามี container ของสินค้าแสดงอยู่หรือไม่
        public bool IsInventoryContainerDisplayed()
        {
            return driver.FindElement(inventoryContainer).Displayed;
        }

        //ค้นหาและเพิ่มสินค้าทั้งหมดที่มีคำว่า "T-Shirt, Backpack, Flashlight" ในลงในรถเข็น
        public void AddProductsToCart(string keyword)
        {
            var container = driver.FindElement(inventoryContainer);
            var items = container.FindElements(productNames);
            var buttons = container.FindElements(addToCartButtons);

            bool foundAny = false;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text.Trim().Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    buttons[i].Click();
                    Console.WriteLine($"Added '{keyword}' to cart at index {i}.");
                    foundAny = true;
                }
            }

            if (!foundAny)
            {
                Console.WriteLine($"Product '{keyword}' not found.");
            }
        }
    }
}
