using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Net.Mail;

namespace Hoodwink
{
    class Program
    {
        static string Email = "";
        static string Password = "";
        static string Code;
        static string LocalStoragevalue = "";
        static bool UseBackupFirst = false;
        static uint Delay = 6000;
        static ChromeDriver driver;

        static void Main(string[] args)
        {
            Console.Title = "Quizizz Bot by CursedSheep#1337 ;D";
            byte[] inputBuffer = new byte[4096];
            Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
            Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        again:
            Console.Clear();
            writeshit();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1 - Live");
            Console.Write("Choose option... ");
            var key = Console.ReadKey();
            if (!HandleSwitch(key.KeyChar))
                goto again;
        }
        private static bool HandleSwitch(char c)
        {
            switch (c)
            {
                case '1':
                    {
                        Console.Clear();
                        writeshit();
                        Console.ForegroundColor = ConsoleColor.Green;
                    eEmail:
                        Console.Write("Enter Email: ");
                        Email = Console.ReadLine();
                        if (!(Email.Length > 0 && IsValidMail(Email))) goto eEmail;
                        ePass:
                        Console.Write("Enter Password: ");
                        Password = ReadPassword();
                        if (Password.Length < 3) goto ePass;
                        eCode:
                        Console.Write("Enter code/link: ");
                        Code = Console.ReadLine();
                        if (!(((Code.Length == 6 || Code.Length == 8) && Code.All(x => char.IsDigit(x))) || (Uri.TryCreate(Code, UriKind.Absolute, out Uri uriResult)
    && uriResult.Scheme == Uri.UriSchemeHttps))) goto eCode;
                        eDelay:
                        Console.Write("Enter delay (Milliseconds): ");
                        if (!uint.TryParse(Console.ReadLine(), out Delay)) goto eDelay;

                        Console.Clear();
                        startshit();
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
        //Starts Browser
        private static void startshit()
        {
            Task.Factory.StartNew(RunTaskThread);
            Thread.Sleep(Timeout.Infinite);
        }
        //Function from stackoverflow
        private static string ReadPassword()
        {
            List<char> pwd = new List<char>();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Count > 0)
                    {
                        pwd.RemoveAt(pwd.Count - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') 
                {
                    pwd.Add(i.KeyChar);
                    Console.Write("*");
                }
            }
            return new string(pwd.ToArray());
        }
        static void writeshit()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"██   ██  ██████   ██████  ██████  ██     ██ ██ ███    ██ ██   ██ 
██   ██ ██    ██ ██    ██ ██   ██ ██     ██ ██ ████   ██ ██  ██  
███████ ██    ██ ██    ██ ██   ██ ██  █  ██ ██ ██ ██  ██ █████   
██   ██ ██    ██ ██    ██ ██   ██ ██ ███ ██ ██ ██  ██ ██ ██  ██  
██   ██  ██████   ██████  ██████   ███ ███  ██ ██   ████ ██   ██");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("# ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Credits: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("~ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("AndyFilter - Method for acquiring answers");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("~ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Yeetret#5557 - Helped with testing");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("# ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Developed by CursedSheep#1337\n");
        }
        static async Task<bool> StartBrowser()
        {
            writeshit();
            Console.ForegroundColor = ConsoleColor.Green;
            var options = new ChromeOptions();
            options.AddArgument("chrome_options=opt,service_log_path='NUL'");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            string URL = "https://quizizz.com/login";
            driver.Navigate().GoToUrl(URL);
            await Task.Delay(5000);
            if (driver.Url.Contains("quizizz.com"))
                return await Login();

            return false;
        }
        static Thread t1;
        static async void RunTaskThread()
        {
            bool result = await StartBrowser();
            if (!result)
            {
                Console.WriteLine("Failed to start browser!");
                Console.ReadKey();
            }
            else
            {
                await EnterCodeOrUrl(Code);
                await StartGame();
                while(true)
                {
                    await Task.Run(() => GetLocalStorage());
                    var roomHash = await AnswerUtils.GetRoomData(LocalStoragevalue);
                    var quizizzInfo = AnswerUtils.GetSetData<QuizizzInfo>($"https://quizizz.com/api/main/quiz/{roomHash.room.quizId}/info");
                    var firstQuestionID = roomHash.room.questions[0];
                    var QuestionsData = AnswerUtils.getAnswerData(roomHash, quizizzInfo);

                    List<QuizItem> answers = null;
                    if(!UseBackupFirst)
                        answers = AnswerUtils.GetAnswersLive(QuestionsData, firstQuestionID);
                    if (answers == null)
                    {
                        Console.WriteLine("Failed acquiring answers from cache! Using backup...");
                        var currentDirectory = Directory.GetCurrentDirectory();
                        var backupPath = Path.Combine(currentDirectory, "backup.txt");
                        while(!File.Exists(backupPath))
                        {
                            Console.WriteLine($"Cannot find {backupPath}! Press enter when the file is present...");
                            Console.ReadKey();
                        }
                        MemoryStream ms = new MemoryStream();
                        using (FileStream fs = new FileStream(backupPath, FileMode.Open, FileAccess.Read))
                        {
                            fs.CopyTo(ms);
                        }
                        string str = Encoding.UTF8.GetString(ms.ToArray());
                        ms.Close();
                        ms.Dispose();
                        var Deserialized = JsonConvert.DeserializeObject<Structures.QuizizzInfo2.Root>(str);
                        answers = AnswerUtils.GetAnswersLive2(Deserialized);
                    }

                    foreach (var item in answers)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Question: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(item.Question);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Answers: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(string.Join(", ", item.Answers.Select(x => AnswerUtils.ConvertToPlainText(x))));
                    }
                    while (TryGetElement(driver, GetID("class", "current-question")) == null) ;
                    checkQuestionCount:
                    var isValid1 = int.TryParse(driver.FindElementByXPath(GetID("class", "current-question")).Text, out int questionNum);
                    var isValid2 = int.TryParse(driver.FindElementByXPath(GetID("class", "total-questions")).Text.TrimStart('/'), out int questionTotal);
                    if (!(isValid1 && isValid2)) goto checkQuestionCount;
                    t1 = new Thread(() => { fuckOffPowerUps(driver); });
                    t1.Start();
                    while (TryGetElement(driver, GetID("class", "main-section-title")) == null)
                    {
                    chkquestion:
                        Thread.Sleep((int)Delay);
                        IWebElement zzzz;
                        string questionValue;
                        try
                        {
                            zzzz = driver.FindElementByXPath(GetID("class", "resizeable question-text-color"));
                            questionValue = zzzz.GetAttribute("innerHTML");
                        }
                        catch
                        {
                            questionValue = "";
                        }
                        var mediaval = TryGetMedia(driver).Split('?')[0];
                        QuizItem answerData;
                        try
                        {
                            answerData = answers.First(x => (x.Question == questionValue || x.Question == questionValue) && x.ImageUrl == mediaval);
                        }
                        catch
                        {
                            goto chkquestion;
                        }
                        try
                        {
                            switch (answerData.Type)
                            {
                                case "MCQ":
                                    {
                                        var choices = driver.FindElementByXPath(GetID("class", "options-grid")).FindElements(By.ClassName("option"));
                                        if (TryGetElement(driver, GetID("class", "option-image")) != null)
                                        {
                                            choices = driver.FindElementByXPath(GetID("class", "options-grid")).FindElements(By.ClassName("option-image"));
                                            var ElementAnswers = choices.Where(x => answerData.Answers.Contains(x.GetAttribute("style").Substring(23, x.GetAttribute("style").Length - 26).Split('?')[0]));
                                            foreach (var item in ElementAnswers)
                                            {
                                                ClickLoopwhilenotPresent(item);
                                            }
                                        }
                                        else
                                        {
                                            var ElementAnswers = GetStringFromList(choices, answerData.Answers);
                                            foreach (var item in ElementAnswers)
                                            {
                                                ClickLoopwhilenotPresent(item);
                                            }
                                        }
                                    }
                                    break;
                                case "MSQ":
                                    {
                                        var choices = driver.FindElementByXPath(GetID("class", "options-grid")).FindElements(By.ClassName("option"));
                                        var submitBtn = driver.FindElementByXPath(GetID("class", "submit-button-text"));
                                        var ElementAnswers = GetStringFromList(choices, answerData.Answers);
                                        foreach (var item in ElementAnswers)
                                        {
                                            ClickLoopwhilenotPresent(item);
                                        }
                                        ClickLoopwhilenotPresent(submitBtn);
                                    }
                                    break;
                                case "BLANK":
                                    {
                                        var submitBtn = driver.FindElementByXPath(GetID("class", "submit-button-text"));
                                        var inputText = driver.FindElementByXPath(GetID("class", "typed-option-input is-incorrect"));
                                        ClickLoopwhilenotPresent(inputText);
                                        inputText.SendKeys(answerData.Answers[0]);
                                        await Task.Delay(1000);
                                        submitBtn.Click();
                                    }
                                    break;
                            }
                        }
                        catch
                        {

                        }
                    }
                    t1.Abort();
                }
            }
        }
        static IWebElement[] GetStringFromList(IReadOnlyCollection<IWebElement> s, string[] t)
        {
            List<IWebElement> z = new List<IWebElement>();
            foreach (var item in s)
            {
                string s2 = item.FindElement(By.ClassName("resizeable")).GetAttribute("innerHTML");
                foreach (var item2 in t)
                {
                    if (s2 == item2)
                        z.Add(item);
                }
            }

            return z.ToArray();
        }
        static async Task<bool> Login()
        {
            //Login function
            string[] CREDS = { Email, Password };
            var emailField = driver.FindElementByXPath(GetID("class", "user-id-field"));
            emailField = emailField.FindElements(By.XPath(GetID("type", "text"))).First(x => x.Displayed == true);
            var passField = driver.FindElementByXPath(GetID("class", "user-pass-field"));
            passField = passField.FindElement(By.XPath(GetID("type", "password")));
            var btn1 = driver.FindElementByXPath(GetID("class", "do-login-btn kr2-btn kr2-btn-disabled"));
            emailField.Click();
            emailField.SendKeys(CREDS[0]);
            await Task.Delay(500);
            passField.Click();
            passField.SendKeys(CREDS[1]);
            await Task.Delay(1000);
            btn1.Click(); //#Sign in button
            await Task.Delay(5000);
            driver.Navigate().GoToUrl("https://quizizz.com/join");
            await Task.Delay(5000);
            return true;

        }
        static async Task<bool> EnterCodeOrUrl(string code)
        {
            if((code.Length == 6 | code.Length == 8) && code.All(x => char.IsDigit(x)))
            {
                var codeInputText = driver.FindElementByXPath(GetID("class", "check-room-input"));
                var codeInputBtn = driver.FindElementByXPath(GetID("class", "check-room-button text-unselectable"));
                codeInputText.SendKeys(code);
                await Task.Delay(500);
                codeInputBtn.Click();
            }
            else driver.Navigate().GoToUrl(code);
            await Task.Delay(5000);
            return true;
        }
        static async Task<bool> StartGame()
        {
            var startbtn = TryGetElement(driver, GetID("class", "primary-button start-game")) ?? TryGetElement(driver, GetID("class", "primary-button resume-game"));
            startbtn.Click();
            await Task.Delay(3000);
            return true;
        }
        static async void GetLocalStorage()
        {
            LocalStoragevalue = driver.WebStorage.LocalStorage.GetItem("previousContext");
            await Task.Delay(3000);
        }
        static OpenQA.Selenium.IWebElement TryGetElement(ChromeDriver driver, string xpath)
        {
            try
            {
                return driver.FindElementByXPath(xpath);
            }
            catch
            {
                return null;
            }
        }
        //Get rid of power-up notifs
        static void fuckOffPowerUps(ChromeDriver driver)
        {
            while(true)
            {
                string[] popUpList = { "powerup-reminder-button", "powerup-reminder-button not-now-button", "not-now-button", "powerup-onboarding-button" };
                try
                {
                    foreach (var s in popUpList)
                    {
                        var e = TryGetElement(driver, GetID("class", s));
                        if (e != null)
                        {
                            Thread.Sleep(50);
                            e.Click();
                        }
                    }
                }
                catch
                {

                }
            }
        }
        static string TryGetMedia(ChromeDriver driver)
        {
            try
            {
                var e = driver.FindElementByXPath(GetID("class", "question-media")).FindElement(By.ClassName("is-clickable"));
                return e.GetAttribute("src");
            }
            catch
            {
                return "";
            }
        }
        //reserved function
        static OpenQA.Selenium.IWebElement ElementLoopwhilenotPresent(ChromeDriver driver, string xpath)
        {
            again:
            try
            {
                var g = driver.FindElementByXPath(xpath);
                _ = g.Text;
                return driver.FindElementByXPath(xpath);
            }
            catch
            {
                goto again;
            }
        }
        static void ClickLoopwhilenotPresent(IWebElement e)
        {
            try
            {
                if(e.Enabled)
                {
                again:
                    try
                    {
                        try
                        {
                            _ = e.Enabled;
                        }
                        catch { return; }
                         e.Click();
                    }
                    catch
                    {
                        goto again;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        private static bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        static string GetID(string objID, string id) => @"//*[@" + objID + "=" + '"' + id + '"' + "]";
    }
}
