using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Retriever
    {
        // Our singleton instance.
        private static Retriever _dataRetriever;

        /// <summary>
        /// The name to save the skater html file under.
        /// </summary>
        private const string SkaterFileName = "Skater.html";

        /// <summary>
        /// The name to save the goalie html file under.
        /// </summary>
        private const string GoalieFileName = "Goalie.html";

        // The web address where the skater data lies.
        private const string SkaterUrl = "http://www.mystatsonline.com/hockey/visitor/league/stats/skater_hockey.aspx?IDLeague=6814";

        // The web address where the goalie data lies.
        private const string GoalieUrl = "http://www.mystatsonline.com/hockey/visitor/league/stats/goalie_hockey.aspx?IDLeague=6814";

        /// <summary>
        /// The name of the file the data is under.
        /// </summary>
        public string FileName = string.Empty;

        /// <summary>
        /// The url for retrieving statistics.
        /// </summary>
        public string Url = string.Empty;

        /// <summary>
        /// The xpath command for accessing headings and statistics.
        /// </summary>
        public string XPathStat = string.Empty;

        /// <summary>
        /// The xpath command for accessing player names.
        /// </summary>
        public string XPathName = string.Empty;

        /// <summary>
        /// Goes out to the web if the file doesn't exist and grabs it's html code.
        /// Will return true of success and will throw an error on failure.
        /// Stores filename, xpath variables, Urls for access outside of class.
        /// </summary>
        /// <param name="playerType"></param>
        /// <returns></returns>
        public void CreateFile(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Skater:
                    FileName = SkaterFileName;
                    Url = SkaterUrl;
                    break;
                default:
                    FileName = GoalieFileName;
                    Url = GoalieUrl;
                    break;
            }

            if (File.Exists(FileName)) return;
            var html = GetHtmlFromBrowser(Url);
            WriteToFile(html, FileName);
        }

        private void WriteToFile(string html, string fileName)
        {            
            using (var sw = new StreamWriter(fileName))
            {
                sw.WriteLine(html);
            }
        }

        /// <summary>
        /// Returns a singleton instance of data retriever
        /// </summary>
        /// <returns></returns>
        public static Retriever GetInstance()
        {
            return _dataRetriever ?? (_dataRetriever = new Retriever());
        }

        /// <summary>
        /// Goes out to the webserver and fetches the html file after running
        /// the necessary javascript commands.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetHtmlFromBrowser(string url)
        {
            // Go to the web address.
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);

            // The javascript code that allows us to get the right statistics.
            List<Script> scripts = new List<Script>
            {
                new Script("#maintitle_ddlSeason", "0"),
                new Script("label > select", "-1"),
            };

            // Run all the scripts.
            foreach (var script in scripts)
            {
                script.RunScript(driver);
                Thread.Sleep(2000);
            }

            string source = driver.PageSource;

            // Close firefox and return the html string
            driver.Quit();
            return source;
        }        
    }
}
