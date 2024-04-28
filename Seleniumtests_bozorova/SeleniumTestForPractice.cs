using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtest_bozorova;

public class SeleniumTestForPractice
{

    public ChromeDriver driver;

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
    }
    
    [TearDown]
    public void TearDown()
    {
        //- закрываем браузер и убиваем процесс драйвера
        driver.Close();
        driver.Quit();
    }
    
    public void Auth(string user, string password)
    {
        driver.Navigate().GoToUrl(staffSite);
        driver.FindElement(By.Id("Username")).SendKeys(user);
        driver.FindElement(By.Name("Password")).SendKeys(password);
        driver.FindElement(By.Name("button")).Click();
        driver.FindElement(By.CssSelector("h1[data-tid='Title']"));
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
        Assert.That(driver.FindElement(By.CssSelector("h1[data-tid='Title']")).Displayed, "Не удалось авторизоваться");
    }

    [Test]
    public void TestMenu()
    {
        // auth не вынесен в SetUp, чтобы для каждого теста можно было указать конкретного пользователя
        Auth("user", "1q2w3e4r%T");
        driver.Navigate().GoToUrl(staffSite);
        driver.FindElement(By.XPath("//span[contains(text(), 'Сообщества')]")).Click();
        Assert.That(driver.FindElement(By.XPath("//h1[contains(text(), 'Сообщества')]")).Displayed, "Не найден заголовок Сообщества");
    }

    [Test]
    public void TestSearch()
    {
        // auth не вынесен в SetUp, чтобы для каждого теста можно было указать конкретного пользователя
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
        // auth не вынесен в SetUp, чтобы для каждого теста можно было указать конкретного пользователя
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
        // auth не вынесен в SetUp, чтобы для каждого теста можно было указать конкретного пользователя
        Auth("user", "1q2w3e4r%T");
        driver.FindElement(By.CssSelector("div[data-tid='Avatar']")).Click();
        driver.FindElement(By.CssSelector("span[data-tid='Logout'] span")).Click();
        var logoutText = driver.FindElement(By.CssSelector("h3")).Text;
        var expectedLogoutText = "Вы вышли из учетной записи";
        Assert.That(logoutText == expectedLogoutText, $"Фактический текст {logoutText} отличается от ожидаемого {expectedLogoutText}");
    }
    
    [Test]
    public void TestPost()
    {
        var testText = "Человек-бензопила";
        var pathToFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../files/chainsawman.jpg"));
        // auth не вынесен в SetUp, чтобы для каждого теста можно было указать конкретного пользователя
        Auth("user", "1q2w3e4r%T");
        driver.Navigate().GoToUrl(staffSite + "/communities/5a9fcb78-c7d9-48d2-af76-8fe21b70d182");
        driver.FindElement(By.CssSelector("div[data-tid='AddButton']")).Click();
        driver.FindElement(By.CssSelector("div[role='textbox']")).SendKeys(testText);
        driver.FindElement(By.CssSelector("button[title='Добавить изображение']")).Click();
        driver.FindElement(By.CssSelector("div[data-tid='PopupContent'] input")).SendKeys(pathToFile);
        driver.FindElement(By.CssSelector("span[data-tid='SendButton'] button")).Click();
        var text = driver.FindElement(By.CssSelector("span[data-text='true']")).Text;
        Assert.That(text == testText, $"Фактический текст {text} отличается от ожидаемого {testText}");
        driver.FindElement(By.CssSelector("div[data-tid='Feed'] div[data-tid='DropdownButton']")).Click();
        driver.FindElement(By.CssSelector("span[data-tid='DeleteRecord']")).Click();
        driver.FindElement(By.CssSelector("span[data-tid='DeleteButton']")).Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        Assert.DoesNotThrow(() => wait.Until(ExpectedConditions.InvisibilityOfElementWithText(By.CssSelector("span[data-text='true']"),testText)), "Пост не удален");
    }
}