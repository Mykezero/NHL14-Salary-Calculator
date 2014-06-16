using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Parser
    {
        /// <summary>
        /// Singleton instance for this class.
        /// </summary>
        private static Parser _playerData;

        /// <summary>
        /// The html source for statistics
        /// </summary>
        private HtmlDocument _document = new HtmlDocument();

        /// <summary>
        /// The table that contains our statistics
        /// </summary>
        private HtmlNodeCollection _stats;

        /// <summary>
        /// The table the contains our player's names.
        /// </summary>
        private HtmlNodeCollection _names;

        private Parser(PlayerType playerType)
        {
            // Create the html file if it doesn't exist
            var fetcher = Retriever.GetInstance();
            fetcher.CreateFile(PlayerType.Skater);

            // Read in the html file.
            _document.Load(fetcher.FileName);

            // Get the table that our statistics are under.
            this._stats = _document.DocumentNode.SelectNodes(fetcher.XPathStat);
            this._names = _document.DocumentNode.SelectNodes(fetcher.XPathName);
        }

        /// <summary>
        /// Provides access to our playerdata instance
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static Parser GetInstance(PlayerType playerType)
        {
            return _playerData == null ? _playerData = new Parser(playerType) : _playerData;
        }

        /// <summary>
        /// Returns a list of player details. 
        /// For example 
        ///     number in dataset,
        ///     player's name,
        ///     player's team
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<String> GetPlayerDetails(int i)
        {
            if (_names == null)
                return new List<string>();

            var names = Clean(_names[i]);

            return names;

        }

        /// <summary>
        /// Returns the player's statistic categories.
        /// For example, Goals, Assists, Goals per game etc.
        /// </summary>
        /// <returns></returns>
        public List<String> GetHeaderFields()
        {
            if (_stats == null)
                return new List<string>();

            var headers = Clean(_stats[0]);

            return headers;
        }

        /// <summary>
        /// Returns the statistics for a single player.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<double> GetStatisticFields(int i)
        {
            if (_stats == null || i < 0 || i >= _stats.Count)
                return new List<double>();

            // Get the stats as strings
            var rawData = Clean(_stats[i]);

            // The list of stats to return
            List<double> stats = new List<double>();

            // For all stats...
            // Add them if they can be converted to ints or ...
            // Make them zero if they are not             
            // -- Since one might be a '-'
            for (int j = 0; j < rawData.Count(); j++)
            {
                var datum = rawData[j];
                double dval = 0;
                int ival = 0;

                // Try to convert value first to int
                if (int.TryParse(datum, out ival))
                    stats.Add(ival);
                // Try to convert to double now
                else if (double.TryParse(datum, out dval))
                    stats.Add(dval);
                // Converts percentages to decimals
                else if (datum.Contains("%"))
                {
                    // Remove the % sign
                    datum = datum.Replace("%", "");

                    // Convert the percent into decimal form.
                    // for example, 35% = 35/100 = .35
                    dval = Math.Round(int.Parse(datum) / 100.0, 2);

                    // Add to stats list
                    stats.Add(dval);
                }
                // Might be a '-' for (N.A.). Make it a 0.
                else
                    stats.Add(0);
            }

            // Return the converted stats
            return stats;
        }

        private List<String> Clean(HtmlNode data)
        {
            // Get the stats as strings
            return data
                    .ChildNodes
                    .Where(x => !String.IsNullOrWhiteSpace(x.InnerText))
                    .Select(x => x.InnerText.Trim())
                    .ToList();
        }

        public String GetPlayerString(int i)
        {
            // Set name and team details
            var details = GetPlayerDetails(i);
            var name = details[1];
            var team = details[2];

            // Stat statistics and headers
            var head = GetHeaderFields();
            var stats = GetStatisticFields(i);

            var record = String.Join("\t", GetPlayerDetails(1));
            record += Environment.NewLine;
            record += String.Join("\t", GetHeaderFields());
            record += Environment.NewLine;
            record += String.Join("\t", GetStatisticFields(1));

            return record;
        }
    }
}