using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Retriever
    {
        // Our singleton instance.
        private static Retriever _dataRetriever;

        /// <summary>
        /// The name to save the skater html file under.
        /// </summary>
        private const String SkaterFileName = "Skater.html";

        /// <summary>
        /// The name to save the goalie html file under.
        /// </summary>
        private const String GoalieFileName = "Goalie.html";

        // The web address where the skater data lies.
        private const String SkaterUrl = "http://www.mystatsonline.com/hockey/visitor/league/stats/skater_hockey.aspx?IDLeague=5487";

        // The web address where the goalie data lies.
        private const String GoalieUrl = "http://www.mystatsonline.com/hockey/visitor/league/stats/goalie_hockey.aspx?IDLeague=5487";

        /// <summary>
        /// The name of the file the data is under.
        /// </summary>
        public string FileName = String.Empty;

        /// <summary>
        /// The url for retrieving statistics.
        /// </summary>
        public String Url = String.Empty;

        /// <summary>
        /// The xpath command for accessing headings and statistics.
        /// </summary>
        public String XPathStat = String.Empty;

        /// <summary>
        /// The xpath command for accessing player names.
        /// </summary>
        public string XPathName = String.Empty;

        /// <summary>
        /// Goes out to the web if the file doesn't exist and grabs it's html code.
        /// Will return true of success and will throw an error on failure.
        /// Stores filename, xpath variables, Urls for access outside of class.
        /// </summary>
        /// <param name="playerType"></param>
        /// <returns></returns>
        public bool CreateFile(PlayerType playerType)
        {
            if (playerType == PlayerType.Skater)
            {
                FileName = SkaterFileName;
                Url = SkaterUrl;
                XPathStat = GetXPath("Skater");
                XPathName = GetXPath("SkaterName");
            }
            else
            {
                FileName = GoalieFileName;
                Url = GoalieUrl;
                XPathStat = GetXPath("Goalie");
                XPathName = GetXPath("GoalieName");
            }

            try
            {
                // Create the html locally if the file does not exist.
                // It will go to the web to get the data and will write it to file.
                if (!File.Exists(FileName))
                {
                    using (StreamWriter sw = new StreamWriter(FileName))
                    {
                        sw.WriteLine(GetHtmlFromBrowser(Url));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// Returns a singleton instance of data retriever
        /// </summary>
        /// <returns></returns>
        public static Retriever GetInstance()
        {
            return _dataRetriever == null ? _dataRetriever = new Retriever() : _dataRetriever;
        }

        /// <summary>
        /// Goes out to the webserver and fetches the html file after running
        /// the necessary javascript commands.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private String GetHtmlFromBrowser(String url)
        {
            // Go to the web address.
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(url);

            // The javascript code that allows us to get the right statistics.
            List<Script> scripts = new List<Script>();
            scripts.Add(new Script("MainContent_ddlSeason", "0"));
            scripts.Add(new Script("MainContent_ddlShow", "0"));
            scripts.Add(new Script("MainContent_ddlPlayer", ""));

            // Run all the scripts.
            foreach (var script in scripts)
            {
                script.RunScript(driver);
                Thread.Sleep(2000);
            }

            String source = driver.PageSource;

            // Close firefox and return the html string
            driver.Quit();
            return source;
        }

        /// <summary>
        /// Returns the XPath address for Goalies and Players
        /// </summary>
        /// <param name="playerType">
        /// Either 'Skater', 'Goalie', 'SkaterName', 'GoalieName'
        /// </param>
        /// <returns>
        /// The XPath command for getting player statistics
        /// </returns>
        private String GetXPath(String playerType)
        {
            return "//table[@id='MainContent_gv" + playerType + "']/tbody/tr";
        }
    }
}
