using System.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtest_bozorova;

public class SeleniumTestForPractice
{

    public ChromeDriver driver;
    public WebDriverWait wait;

    //убирала домен в переменную
    public string staffSite = "https://staff-testing.testkontur.ru";

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        driver = new ChromeDriver(options);
        // - настраиваем неявное ожидание в 15 секунд
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    public void Authorization()
    {
        // -перейти по урлу https://staff-testing.testkontur.ru
        driver.Navigate().GoToUrl(staffSite);

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

        // - проверяем что находимся на нужной странице
        Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")).Displayed);
    }

    [TearDown]
    public void TearDown()
    {
        //- закрываем браузер и убиваем процесс драйвера
        driver.Close();
        driver.Quit();
    }

    [Test]
    public void TestMenu()
    {
        Auth("user", "1q2w3e4r%T");
        driver.Navigate().GoToUrl(staffSite);
        driver.FindElement(By.XPath("//span[contains(text(), 'Сообщества')]")).Click();
        Assert.That(driver.FindElement(By.XPath("//h1[contains(text(), 'Сообщества')]")).Displayed, "Не найден заголовок Сообщества");
    }
    

    public void Auth(string user, string password)
    {
        driver.Navigate().GoToUrl(staffSite);
        driver.FindElement(By.Id("Username")).SendKeys(user);
        driver.FindElement(By.Name("Password")).SendKeys(password);
        driver.FindElement(By.Name("button")).Click();
        Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")).Displayed, "Не удалось авторизоваться");
    }

    [Test]
    public void TestSearch()
    {
        Auth("user","1q2w3e4r%T");
        driver.Navigate().GoToUrl(staffSite);
        driver.FindElement(By.CssSelector("span[data-tid='SearchBar']")).Click();
        driver.FindElement(By.CssSelector("span[data-tid='SearchBar'] input")).SendKeys("Бозорова");
        driver.FindElement(By.CssSelector("div[title='Милана Бозорова']")).Click();
        var employeeName = driver.FindElement(By.CssSelector("div[data-tid='EmployeeName']")).Text;
        var expectedEmployeeName = "Милана Бозорова";
        Assert.That(employeeName == expectedEmployeeName,$"Фактическое имя {employeeName} отличается от ожидаемого {expectedEmployeeName}");
    }

    [Test]

    public void TestCommunity()
    {
        Auth("user","1q2w3e4r%T");
        driver.Navigate().GoToUrl(staffSite + "/communities");
        driver.FindElement(By.XPath("//button[contains(text(), 'СОЗДАТЬ')]")).Click();
        driver.FindElement(By.CssSelector("textarea[placeholder='Название сообщества']")).SendKeys("Тест");
        driver.FindElement(By.CssSelector("textarea[placeholder='Описание сообщества']")).SendKeys("Тест");
        driver.FindElement(By.XPath("//span[contains(text(), 'Создать')]")).Click();
        driver.FindElement(By.CssSelector("button[data-tid='DeleteButton']")).Click();
        driver.FindElement(By.XPath("//span[contains(text(), 'Удалить')]")).Click();
    }

    [Test]
    public void TestLogout()
    {
        Auth("user", "1q2w3e4r%T");
        driver.Navigate().GoToUrl(staffSite + "/news");
        driver.FindElement(By.CssSelector("div[data-tid='Avatar']")).Click();
        driver.FindElement(By.CssSelector("span[data-tid='Logout'] span")).Click();
        var logoutText = driver.FindElement(By.CssSelector("h3")).Text;
        var expectedLogoutText = "Вы вышли из учетной записи";
        Assert.That(logoutText == expectedLogoutText, $"Фактический текст {logoutText} отличается от ожидаемого {expectedLogoutText}");
    }
    
    
}