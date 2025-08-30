using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using SauceDemoTest.Pages;
using SauceDemoTest.Utils;   
using System;
using System.IO;

namespace SauceDemoTest
{
    [TestFixture]
    public class Testing
    {
        IWebDriver? driver;
        Testdata? testdata;
        Logger? logger;   

        [SetUp]
        public void SetUp()
        {
            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();

            // Load configuration
            var configText = File.ReadAllText("data.json");
            testdata = JsonConvert.DeserializeObject<Testdata>(configText)
                       ?? throw new InvalidOperationException("Failed to load test data.");

            // บันทึกเป็น text file
            logger = new Logger("test_log.txt");
        }

        [Test]
        public void SuaceDemoUITests()
        {
            driver!.Navigate().GoToUrl(testdata!.baseUrl!.url);
            logger!.Log($"เปิดเว็บไซต์: {testdata.baseUrl.url}");

            var loginPage = new Pages.LoginPage(driver);
            loginPage.Login(testdata.User[0].username, testdata.User[0].password);
            logger.Log("เข้าสู่ระบบสำเร็จ");

            Assert.IsTrue(driver.Url.Contains("inventory.html"));
            logger.Log("ตรวจสอบการเข้าสู่ระบบสำเร็จแล้ว");

            // Inventory Page
            var inventoryPage = new Pages.InventoryPage(driver);
            var product = testdata.Product[0];

            if (inventoryPage.IsInventoryContainerDisplayed())
            {
                logger.Log("แสดงรายการสินค้าเรียบร้อย");

                inventoryPage.AddProductsToCart(product.tshirt);
                inventoryPage.AddProductsToCart(product.backpack);
                inventoryPage.AddProductsToCart(product.flashlight);
                logger.Log("เพิ่มสินค้าใส่ตะกร้าแล้ว");
            }
            else
            {
                logger.Log("ไม่พบสินค้าที่ต้องการ");
            }

            // Cart Page
            var cartPage = new Pages.CartPage(driver);
            cartPage.GoToCart();
            logger.Log("ไปที่หน้าตะกร้าสินค้า");

            cartPage.RemoveProductsFromCart();
            logger.Log("ลบสินค้า 'Backpack' ออกจากตะกร้า");

            //Assert.IsTrue(!cartPage.GetCartItems().Contains(product.backpack));
            //logger.Log("ตรวจสอบการลบ Backpack ออกจากตะกร้าเรียบร้อย");

            // ตรวจสอบว่ามี product.tshirt อยู่ในตะกร้า 2 ชิ้น
            int tshirtCount = cartPage.GetCartItems()
                .Count(item => item.Contains(product.tshirt, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(2, tshirtCount, $"Expected 2 '{product.tshirt}' in cart, but found {tshirtCount}.");


            cartPage.ClickCheckout();
            logger.Log("กดปุ่มชำระเงิน");

            // Checkout Page
            var checkoutPage = new Pages.CheckoutPage(driver);
            checkoutPage.EnterCheckoutInformation(
                testdata.Checkout[0].firstname,
                testdata.Checkout[0].lastname,
                testdata.Checkout[0].postalcode
            );
            logger.Log("กรอกข้อมูลการชำระเงินเรียบร้อย");

            checkoutPage.CalculateAndVerifyTotal();
            logger.Log("ตรวจสอบราคาเรียบร้อย");

            checkoutPage.FinishCheckout();
            logger.Log("ทำรายการสั่งซื้อสำเร็จ");

            Assert.IsTrue(checkoutPage.IsPurchaseSuccessful());
            logger.Log("การซื้อสินค้าสำเร็จระบบแสดงข้อความ Thank you for your order!");
        }

        [TearDown]
        public void TearDown()
        {
            logger?.Dispose();
            driver?.Quit();
            driver?.Dispose();
            driver = null;
        }
    }
}

