using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlAgilityPack;

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
        private readonly HtmlDocument _document = new HtmlDocument();

        /// <summary>
        /// The table that contains our statistics
        /// </summary>
        private readonly HtmlNodeCollection _stats;

        /// <summary>
        /// The table the contains our player's names.
        /// </summary>
        private readonly HtmlNodeCollection _names;

        private Parser(PlayerType playerType)
        {
            // Create the html file if it doesn't exist
            var fetcher = Retriever.GetInstance();
            fetcher.CreateFile(playerType);

            // Read in the html file.
            _document.Load(fetcher.FileName);

            // Get the table that our statistics are under.
            _stats = _document.DocumentNode.SelectNodes("//table[@id='maincontent_gvGridViewPlayers_gvPlayers']/thead/tr");
            _names = _document.DocumentNode.SelectNodes("//table[@id='maincontent_gvGridViewPlayers_gvPlayers']/tbody/tr");
        }

        /// <summary>
        /// Provides access to our playerdata instance
        /// </summary>
        /// <returns></returns>
        public static Parser GetInstance(PlayerType playerType)
        {
            return _playerData ?? (_playerData = new Parser(playerType));
        }

        /// <summary>
        /// Returns a list of player details. 
        /// For example 
        ///     number in dataset,
        ///     player's name,
        ///     player's team
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        private List<string> GetPlayerStats(HtmlNode htmlNode)
        {
            if (_names == null) return new List<string>();
            var names = Clean(htmlNode);
            return names;
        }

        /// <summary>
        /// Returns the player's statistic categories.
        /// For example, Goals, Assists, Goals per game etc.
        /// </summary>
        /// <returns></returns>
        public List<string> GetHeaderFields()
        {
            if (_stats == null)
                return new List<string>();

            var headers = Clean(_stats[0]);

            return headers;
        }        

        private List<string> Clean(HtmlNode data)
        {
            return data.ChildNodes
                .Where(x => !string.IsNullOrWhiteSpace(x.InnerText))
                    .Select(x => x.InnerText.Trim())
                    .ToList();
        }

        public HockeyStats ViewModel => new HockeyStats
        {
            Headers = new ObservableCollection<string>(GetHeaderFields()),
            Skaters = new ObservableCollection<Skater>(GetSkaters())
        };

        private List<Skater> GetSkaters()
        {
            var skaters = _names.ToList()
                .Select(GetPlayerStats)
                .Where(x => x.Count == GetHeaderFields().Count)
                .Select(x => new Skater(x));
            return skaters.ToList();
        }
    }
}