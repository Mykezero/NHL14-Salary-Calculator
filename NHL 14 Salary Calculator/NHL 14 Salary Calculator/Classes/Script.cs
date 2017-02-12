using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NHL_14_Salary_Calculator.Classes
{
    public class Script
    {
        /// <summary>
        /// Name of the script to run
        /// </summary>
        private readonly string _name;
        
        /// <summary>
        /// The value for the option to select
        /// </summary>
        private readonly string _value;

        public Script(string name, string value)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// Performs a task on the webpage so that we can update our html file for parsing. 
        /// Used for selecting values in java script select statements so that we can cause 
        /// events to fire.
        /// </summary>
        /// <param name="driver">The driver we are using</param>
        public void RunScript(IWebDriver driver)
        {
            var element = driver.FindElement(By.CssSelector(_name));
            SelectElement selectElem = new SelectElement(element);
            selectElem.SelectByValue(_value);
        }
    }
}
