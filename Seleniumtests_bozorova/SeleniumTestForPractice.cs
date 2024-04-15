using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtest_bozorova;

public class SeleniumTestForPractice
{

    public ChromeDriver driver;
    
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        // -звйти в хром ( с помощью вебдрайвера)
        driver = new ChromeDriver(options);
        
        //Неявное ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        
        //ожидания появления страницы
        
        // -перейти по урлу https://staff-testing.testkontur.ru
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");

        //явные ожидания
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Username")));
        
       
        //-ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("user");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("1q2w3e4r%T");
        
        
        // -нажать на кнопку "войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        //
        
        // Thread.Sleep(3000);
        
        // - проверяем что находимся на нужной странице
        // var currentUrl = driver.Url;
        // Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news", $"Фактический URL {currentUrl}");
        
        Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")) is not null);

        //- закрываем браузер и убиваем процесс драйвера
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }

    [Test]
    public void TestMenu()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        driver.FindElement(By.Id("Username")).SendKeys("user");
        driver.FindElement(By.Name("Password")).SendKeys("1q2w3e4r%T");
        driver.FindElement(By.Name("button")).Click();
        Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")) is not null);
        driver.FindElement(By.XPath("//span[contains(text(), 'Сообщества')]")).Click();
        Assert.That(driver.FindElement(By.XPath("//h1[contains(text(), 'Сообщества')]")) is not null);
    }
}