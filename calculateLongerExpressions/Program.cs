using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;

namespace calculateLongerExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            bool displayMenu = true;
            do
            {
                displayMenu = MainMenu();
            } while (displayMenu);
        }

        private static bool MainMenu()
        {
            Console.WriteLine("Enter an equation to solve (use +, -, *, or /) or 'exit' to quit.");

            // store user entry
            string problem = Console.ReadLine();

            // matches every number and operator, but no white space
            var operation = new Regex(@"([+*/-])");
            var matchOperation = operation.Match(problem);

            // Count of operators for the length of the for loop
            int operatorCount = operation.Matches(problem).Count;

            //*************************************
            // MIGHT HAVE TO TAKE OUT NAMES OF MATCHES FOR EASE OF FOR LOOPING LATER
            //*************************************

            // initial regex string since it will need to be this at minimum
            string regexSearch = "^\\s*([0-9\\.,]+)\\s*([+\\-*/]{1})\\s*([0-9\\.,]+)\\s*";

            // for loop to add extra searches based on the number of operators the user input
            for (int i = 2; i < (operatorCount + 1); i++)
            {
                regexSearch += "([+\\-*/]*)\\s*([0-9\\.,]*)\\s*";
            }

            // regex string to gather all operators and numbers
            var regexItem = new Regex(@"" + regexSearch + "");
            var matchItem = regexItem.Match(problem);

            string userProblem = "";
            
            // for loop to construct userProblem string without white space
            for (int i = 1; i < (operatorCount * 2 + 2); i++)
            {
                userProblem += matchItem.Groups[i].Value;
            }

            Console.WriteLine(userProblem);
            return true;
            
        }
    }
}
