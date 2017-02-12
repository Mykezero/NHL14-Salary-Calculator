using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ciloci.Flee;
using NHL_14_Salary_Calculator.Classes;

namespace NHL_14_Salary_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly HockeyStats model;
        private Dictionary<object, bool?> results = new Dictionary<object, bool?>();

        public MainWindow()
        {
            InitializeComponent();
            model = App.HockeyStats;
            DataContext = model.Skaters.Select(x => x.Stats).ToList();

            CalculateButton.Click += CalculateButton_Click;
            HockeyStats.Items.Filter = Filter;
            Formula.KeyDown += Formula_KeyDown;
            KeyDown += MainWindow_KeyDown;
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
                GenerateResults();
            }
        }

        private bool Filter(object x)
        {
            if (string.IsNullOrWhiteSpace(Formula.Text)) return true;
            var result = results.ContainsKey(x) ? results[x] : true;            
            return result ?? true;
        }

        private bool? TryEvaluateExpression(string expression, ExpressionContext context)
        {
            try
            {
                var result = EvaluateExpression(expression, context);
                return result;
            }
            catch (ExpressionCompileException)
            {
                return null;
            }
        }

        private bool EvaluateExpression(string expression, ExpressionContext context)
        {
            IGenericExpression<bool> eGeneric = context.CompileGeneric<bool>(expression);
            var result = eGeneric.Evaluate();
            return result;
        }        

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateResults();
        }

        private void GenerateResults()
        {
            var expression = Formula.Text;

            if (string.IsNullOrWhiteSpace(expression))
            {
                ShowAllStats();
                return;
            }

            var tasks = CreateTasks(expression).ToList();
            tasks.ForEach(x => x.Start());
            results = WaitForResults(tasks);

            ApplyFilter();
        }

        private void ShowAllStats()
        {
            results = model.Skaters.Select(x => x.Stats).ToDictionary(x => (object) x, x => new bool?(true));
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            HockeyStats.Items.Filter = Filter;
            HockeyStats.Items.Refresh();
        }

        private static Dictionary<object, bool?> WaitForResults(IEnumerable<Task<TaskResult>> tasks)
        {
            return Task.WhenAll(tasks)
                .GetAwaiter()
                .GetResult()
                .ToDictionary(x => x.Reference, x => x.Result);
        }

        private IEnumerable<Task<TaskResult>> CreateTasks(string expression)
        {
            return model.Skaters
                .Select(x => x.Stats)
                .Select(x => new Task<TaskResult>(() =>
                {
                    var result = EvaluateExpression(x, expression);
                    return new TaskResult()
                    {
                        Reference = x,
                        Result = result
                    };
                }));
        }

        private bool? EvaluateExpression(Stats x, string expression)
        {
            ExpressionContext context = new ExpressionContext();
            context.Imports.AddType(typeof(Math));
            context.Variables["stats"] = x;
            var result = TryEvaluateExpression(expression, context);
            return result;
        }
    }
}
