using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using SauceDemoTest.Pages;
using System.IO;
using System.Collections.Generic;

namespace SauceDemoTest
{
    [TestFixture]
    public class PurchaseFlowTests
    {
        private IWebDriver driver;
        private Config config;
        private InventoryPage inventoryPage;
        private CartPage cartPage;
        private CheckoutPage checkoutPage;

        public class Config
        {
            public List<string> products { get; set; }
            public double tax { get; set; }
            public User user { get; set; }
            public CheckoutInfo checkoutInfo { get; set; }
        }

        public class User { public string username { get; set; } public string password { get; set; } }
        public class CheckoutInfo { public string firstName { get; set; } public string lastName { get; set; } public string zip { get; set; } }

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            var jsonText = File.ReadAllText("config.json");
            config = JsonConvert.DeserializeObject<Config>(jsonText);
            inventoryPage = new InventoryPage(driver);
            cartPage = new CartPage(driver);
            checkoutPage = new CheckoutPage(driver);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test]
        [Description("ทดสอบการซื้อสินค้าครบวงจร: Login → เพิ่มสินค้า → ลบ Backpack → Checkout → Verify")]
        public void E2E_Purchase_Complete_Flow_Test()
        {
            // 🔐 Login
            PerformLogin();

            // 🛒 Add Products to Cart
            var (cartProducts, notFoundProducts) = AddProductsToCart();

            // 🗑️ Remove Backpack from Cart
            RemoveBackpackIfExists(cartProducts);

            // 💳 Complete Checkout Process
            CompleteCheckout();

            // ✅ Verify Order Success
            VerifyOrderCompletion();

            // 📝 Save Test Results
            SaveTestResults(cartProducts, notFoundProducts);
        }

        private void PerformLogin()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            var loginPage = new LoginPage(driver);
            loginPage.Login(config.user.username, config.user.password);
            Assert.That(driver.Url, Does.Contain("inventory"), "Should navigate to inventory page after login");
        }

        private (List<string> added, List<string> notFound) AddProductsToCart()
        {
            var cartProducts = new List<string>();
            var notFoundProducts = new List<string>();

            foreach (var product in config.products)
            {
                bool added = inventoryPage.AddProduct(product);
                if (added)
                    cartProducts.Add(product);
                else
                    notFoundProducts.Add(product);
            }

            return (cartProducts, notFoundProducts);
        }

        private void RemoveBackpackIfExists(List<string> cartProducts)
        {
            inventoryPage.GoToCart();

            if (cartProducts.Contains("Backpack"))
            {
                cartPage.RemoveProduct("Backpack");
                cartProducts.Remove("Backpack");
            }
        }

        private void CompleteCheckout()
        {
            cartPage.Checkout();
            checkoutPage.FillCheckoutInfo(
                config.checkoutInfo.firstName,
                config.checkoutInfo.lastName,
                config.checkoutInfo.zip
            );
            checkoutPage.FinishOrder();
        }

        private void VerifyOrderCompletion()
        {
            Assert.That(checkoutPage.GetCompleteHeader(),
                       Is.EqualTo("Thank you for your order!"),
                       "Order completion message should be displayed");
        }

        private void SaveTestResults(List<string> cartProducts, List<string> notFoundProducts)
        {
            var log = new
            {
                TestName = "E2E Purchase Complete Flow Test",
                AddedProducts = cartProducts,
                NotFoundProducts = notFoundProducts,
                ProductsCount = cartProducts.Count,
                Timestamp = System.DateTime.Now,
                Status = "PASSED"
            };

            File.WriteAllText("TestResult.json", JsonConvert.SerializeObject(log, Formatting.Indented));
        }
    }
}