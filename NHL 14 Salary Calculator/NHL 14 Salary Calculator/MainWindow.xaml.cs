using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NHL_14_Salary_Calculator.Classes;

namespace NHL_14_Salary_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HockeyStats model;

        public MainWindow()
        {
            InitializeComponent();
            model = App.HockeyStats;
            DataContext = model.Skaters.Select(x => x.Stats).ToList();

            CalculateButton.Click += CalculateButton_Click;
            HockeyStats.Items.Filter = Filter;
            Formula.KeyDown += Formula_KeyDown;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.L || (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;
            Formula.Focus();
        }

        private void Formula_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {            
            if (e.Key == Key.Enter)
            {
                HockeyStats.Items.Filter = Filter;
            }
        }        

        private bool Filter(object x)
        {
            //var tokens = Formula.Text.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);
            //if (!tokens.Any()) return true;

            //var fields = tokens.Where(token => model.Headers
            //    .Select(header => header.ToLowerInvariant())
            //    .Contains(token.ToLowerInvariant()))
            //    .ToDictionary(
            //        header => header,
            //        header => model.Headers
            //            .Select(h => h.ToLowerInvariant())
            //            .ToList()
            //            .IndexOf(header.ToLowerInvariant()));

            //var value = fields.First().Value;

            //var result = (((Stats) x).Map[value] is int ? (int) ((Stats) x).Map[value] : 0) > 50;
            var stats = x as Stats;

            var stdPm = CalculateStdDev(model.Skaters.Select(s => s.Stats.PlusMinus * 1.0 / s.Stats.GamesPlayed).ToList());
            var stdApg = CalculateStdDev(model.Skaters.Select(s => s.Stats.AssistsPerGame).ToList());
            var stdPim = CalculateStdDev(model.Skaters.Select(s => 1.0 * s.Stats.Penalties / s.Stats.GamesPlayed).ToList());
            var pim = 1.0* stats.Penalties/ stats.GamesPlayed;
            var result = stdPm - stats.PlusMinus*1.0/stats.GamesPlayed;


            //return (stats.Position == "D" || stats.Position == "LD" || stats.Position == "RD") &&
            //       (stats.GamesPlayed > 40) &&
            //       (stdApg - stats.AssistsPerGame > 0) &&
            //       (stdPim - pim < 0) &&
            //       (stdPm - result < 0);

            return Regex.IsMatch(stats.Name, Formula.Text, RegexOptions.IgnoreCase);
        }

        private double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }


        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyStats.Items.Filter = Filter;
            HockeyStats.Items.Refresh();
        }

        private DataGridColumn CreateDataGridColumn(string header)
        {
            return new DataGridTextColumn()
            {
                Header = header
            };
        }
    }
}
