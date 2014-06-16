using HtmlAgilityPack;
using NHL_14_Salary_Calculator.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace NHL_14_Salary_Calculator
{
    /// <summary>
    /// The type of player html we'll be parsing
    /// </summary>
    public enum PlayerType
    {
        Skater,
        Goalie
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static String Text = String.Empty;
    
        public App()
        {   
            Text = Parser
                .GetInstance(PlayerType.Skater)
                .GetPlayerString(1);
        }       
    }
}
