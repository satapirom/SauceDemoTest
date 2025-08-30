using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SauceDemoTest.Pages
{
    public class CartPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        // Constructor
        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        // Locators
        private By shoppingCart = By.Id("shopping_cart_container");
        private By cartContainer = By.ClassName("cart_list");
        private By cartItems = By.ClassName("cart_item"); 
        private By itemName = By.ClassName("inventory_item_name");
        private By checkoutButton = By.Id("checkout");

        // Methods
        // คลิกที่ไอคอนรถเข็นเพื่อไปยังหน้ารถเข็น
        public void GoToCart() { 
            
            var cartButton = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(shoppingCart)); 
            
            cartButton.Click(); 
            wait.Until(d => d.Url.Contains("cart.html")); 
        }

        // ลบสินค้า Backpack ออกจากรถเข็น 
        public void RemoveProductsFromCart(string productToRemove = "Backpack")
        {
            var items = driver.FindElements(cartItems);

            foreach (var item in items)
            {
                var nameElement = item.FindElement(itemName);
                if (nameElement.Text.Trim().Contains(productToRemove, StringComparison.OrdinalIgnoreCase))
                {
                    var removeButton = item.FindElement(By.TagName("button"));
                    removeButton.Click();
                    Console.WriteLine($"Removed '{productToRemove}' from cart.");
                    break;
                }
            }
        }
        // ตรวจสอบรายการสินค้าในรถเข็นจะต้องมี T-Shirt 2 ชิ้น 
        public List<string> GetCartItems()
        {
            return driver.FindElements(cartItems)
                         .Select(item => item.FindElement(itemName).Text)
                         .ToList();
        }

        // คลิกปุ่ม Checkout

        public void ClickCheckout()
        {
            driver.FindElement(checkoutButton).Click();
        }
    }
}