# SauceDemo Automation Tests

โปรเจกต์นี้เป็น **Automation Test** สำหรับเว็บไซต์ [SauceDemo](https://www.saucedemo.com/) โดยใช้ **Selenium WebDriver** + **NUnit** บน .NET 8.

---

## สิ่งที่ต้องมี

- .NET 8 SDK
- Visual Studio 2022 หรือสูงกว่า / VS Code
- Browser (สำหรับรัน Selenium)

---

## การติดตั้งและรันเทสต์

### command
Restore NuGet Packages
dotnet restore
Build โปรเจกต์
dotnet build
dotnet test

### โครงสร้างโปรเจกต์
Pages/ - Page Object Model
Tests/ - Test Cases
data.json - ข้อมูลสำหรับรันเทสต์ (Copy อัตโนมัติไป Output)

### Packages 
Selenium.WebDriver --	ควบคุมเว็บเบราว์เซอร์
DotNetSeleniumExtras.WaitHelpers	-- จัดการ Explicit Wait
NUnit	Framework -- เขียน Test
NUnit3TestAdapter	Visual Studio -- รัน NUnit Test
coverlet.collector --	เก็บ Code Coverage
NUnit.Analyzers	-- ตรวจสอบคุณภาพโค้ด Test
