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

            //*****************************
            //adding the $ isn't working for some reason
            //*****************************
            // add $ to end to make sure there are no trailing operators
            regexSearch += "$";

            // regex string to gather all operators and numbers
            var regexItem = new Regex(@"" + regexSearch + "");
            var matchItem = regexItem.Match(problem);

            if (problem.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            else if (matchItem.Success)
            {
                List<string> userProblem = new List<string>();

                // for loop to construct userProblem string without white space
                for (int i = 1; i < (operatorCount * 2 + 2); i++)
                {
                    userProblem.Add(matchItem.Groups[i].Value);
                }

                string question = "";
                // takes out white space for a prettier question equation
                for (int i = 0; i < userProblem.Count; i++)
                {
                    question += (userProblem[i] + " ");
                }

                List<string> needsAddSubtract = calculateMD(userProblem);

                float answer = calculateAS(needsAddSubtract);

                Console.WriteLine("The answer to {0} is {1:#,#0.#####}.", question, answer);

                return true;
            }

            else
            {
                Console.WriteLine("Your entry is invalid. Please only enter numbers and operators. :)");
                return true;
            }
        }

        public static List<string> calculateMD(List<string> userProblem)
        {
            List<string> doubleProblem = new List<string>();

            // int for how often the switch hits * and /
            int countHappens = 1;

            for (int i = 0; i < userProblem.Count; i++)
            {
                switch(userProblem[i])
                {
                    case "*":
                        // change numbers around operator to floats from strings
                        float a = float.Parse(doubleProblem[i - countHappens], CultureInfo.InvariantCulture.NumberFormat);
                        float b = float.Parse(userProblem[i + 1], CultureInfo.InvariantCulture.NumberFormat);
                        // multiplies the numbers around the *
                        float multiply = a * b;
                        // removes the previous number from the list since it was used in the equation above
                        doubleProblem.RemoveAt(i - countHappens);
                        countHappens++;
                        // removes the next number in the string since it was used in the equation above
                        userProblem.RemoveAt(i+1);
                        // *************might have to do i++ depending on if it moves all following items back an interation
                        // convert floats back to strings
                        string strMultiply = Convert.ToString(multiply);
                        // adds the answer to the doubleProblem list for future adding and subtracting
                        doubleProblem.Add(strMultiply);
                        break;
                    case "/":
                        // change number strings around / to floats for solving
                        float c = float.Parse(doubleProblem[i - countHappens], CultureInfo.InvariantCulture.NumberFormat);
                        float d = float.Parse(userProblem[i + 1], CultureInfo.InvariantCulture.NumberFormat);
           
                        // divides the numbers around the /
                        float divide = c / d;
                        // removes the previous number from the list since it was used in the equation above
                        doubleProblem.RemoveAt(i - countHappens);
                        countHappens++;
                        // removes the next number in the string since it was used in the equation above
                        userProblem.RemoveAt(i + 1);
                        // *************might have to do i++ depending on if it moves all following items back an interation
                        // convert floats back to strings
                        string strDivide = Convert.ToString(divide);
                        // adds the answer to the doubleProblem list for future adding and subtracting
                        doubleProblem.Add(strDivide);
                        break;
                    default:
                        doubleProblem.Add(userProblem[i]);
                        break;
                } // end of switch
            } // end of for loop through switch
            return doubleProblem;
        } // end of calculateMD method
        public static float calculateAS(List<string> needsAddSubtract)
        {
            float answer = float.Parse(needsAddSubtract[0], CultureInfo.InvariantCulture.NumberFormat);
 
            for (int i = 0; i < needsAddSubtract.Count; i++)
            {
                if (needsAddSubtract[i] == "+")
                {
                    answer += float.Parse(needsAddSubtract[i + 1], CultureInfo.InvariantCulture.NumberFormat);
                }

                else if (needsAddSubtract[i] == "-")
                {
                    answer -= float.Parse(needsAddSubtract[i + 1], CultureInfo.InvariantCulture.NumberFormat);
                }

            } // end of for loop for switch

            return answer;
        } // end of calculatAS method
    }
}
