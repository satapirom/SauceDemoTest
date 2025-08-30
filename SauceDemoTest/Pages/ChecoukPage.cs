using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SauceDemoTest.Pages
{
    public class CheckoutPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        // Constructor
        public CheckoutPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Elements
        private By firstNameInput = By.Id("first-name");
        private By lastNameInput = By.Id("last-name");
        private By postalCodeInput = By.Id("postal-code");
        private By continueButton = By.Id("continue");

        private By cartItems = By.ClassName("cart_item");
        private By itemName = By.ClassName("inventory_item_name");
        private By itemPrice = By.ClassName("inventory_item_price");

        private By itemTotalLabel = By.ClassName("summary_subtotal_label");
        private By taxLabel = By.ClassName("summary_tax_label");
        private By totalLabel = By.ClassName("summary_total_label");

        private By finishButton = By.Id("finish");

        private By succefulMessage = By.ClassName("complete-header");

        // Methods
        public void EnterCheckoutInformation(string firstName, string lastName, string postalCode)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(firstNameInput)).SendKeys(firstName);
            driver.FindElement(lastNameInput).SendKeys(lastName);
            driver.FindElement(postalCodeInput).SendKeys(postalCode);
            driver.FindElement(continueButton).Click();
        }

        // ตรวจสอบรายการสินค้าและราคารวม + ภาษี
        public bool CalculateAndVerifyTotal(double taxRate = 0.08)
        {
            var items = driver.FindElements(cartItems);

            double sum = 0;
            foreach (var item in items)
            {
                string priceText = item.FindElement(itemPrice).Text;
                double price = double.Parse(priceText.Replace("$", ""), CultureInfo.InvariantCulture);
                sum += price;
            }

            // ราคารวม + ภาษี
            double expectedTax = Math.Round(sum * taxRate, 2);
            double expectedTotal = Math.Round(sum + expectedTax, 2);

            // ดึง text จากหน้าเว็บ
            string itemTotalText = driver.FindElement(itemTotalLabel).Text; // "Item total: $29.99"
            string taxText = driver.FindElement(taxLabel).Text; // "Tax: $2.40"
            string totalText = driver.FindElement(totalLabel).Text; // "Total: $32.39"

            double actualItemTotal = double.Parse(itemTotalText.Split('$')[1], CultureInfo.InvariantCulture);
            double actualTax = double.Parse(taxText.Split('$')[1], CultureInfo.InvariantCulture);
            double actualTotal = double.Parse(totalText.Split('$')[1], CultureInfo.InvariantCulture);

            // ตรวจสอบว่าตรงกันหรือไม่
            bool isCorrect = actualItemTotal == sum && actualTax == expectedTax && actualTotal == expectedTotal;

            if (!isCorrect)
            {
                Console.WriteLine($"Expected Item Total: {sum}, Actual: {actualItemTotal}");
                Console.WriteLine($"Expected Tax: {expectedTax}, Actual: {actualTax}");
                Console.WriteLine($"Expected Total: {expectedTotal}, Actual: {actualTotal}");
            }

            return isCorrect;
        }

        // คลิกปุ่ม Finish 
        public void FinishCheckout()
        {
            driver.FindElement(finishButton).Click();
            wait.Until(d => d.Url.Contains("checkout-complete.html"));
        }

        // ตรวจสอบว่ามีข้อความ "THANK YOU FOR YOUR ORDER" แสดงขึ้นมาหรือไม่
        public bool IsPurchaseSuccessful()
        {
            try
            {
                var messageElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(succefulMessage));
                return messageElement.Text.Contains("THANK YOU FOR YOUR ORDER", StringComparison.OrdinalIgnoreCase);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
