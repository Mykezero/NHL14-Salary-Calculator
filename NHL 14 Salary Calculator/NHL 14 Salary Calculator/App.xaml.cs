using System.Collections.ObjectModel;
using System.Windows;
using NHL_14_Salary_Calculator.Classes;

namespace NHL_14_Salary_Calculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HockeyStats HockeyStats { get; set; }            

        public App()
        {
            HockeyStats = Parser.GetInstance(PlayerType.Skater).ViewModel;
        }
    }

    public class HockeyStats
    {
        public ObservableCollection<string> Headers { get; set; } =
            new ObservableCollection<string>();

        public ObservableCollection<Skater> Skaters { get; set; } =
            new ObservableCollection<Skater>();
    }
}
