using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Script
    {
        /// <summary>
        /// Name of the script to run
        /// </summary>
        private string _name;
        
        /// <summary>
        /// The value for the option to select
        /// </summary>
        private string _value;

        public Script(String name, String value)
        {
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Performs a task on the webpage so that we can update our html file for parsing. 
        /// Used for selecting values in java script select statements so that we can cause 
        /// events to fire.
        /// </summary>
        /// <param name="driver">The driver we are using</param>
        /// <param name="id">The id of the java script function we want to run</param>
        /// <param name="value">The value for the option we want</param>
        public void RunScript(IWebDriver driver)
        {
            var element = driver.FindElement(By.Id(this._name));
            SelectElement selectElem = new SelectElement(element);
            selectElem.SelectByValue(this._value);
        }
    }
}
