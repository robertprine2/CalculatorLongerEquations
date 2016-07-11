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
            // allows user to keep making calculations until they type "exit"
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
            // had to take out variable names of matches for ease of looping
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
            
            // user types "exit" quit the program 
            if (problem.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            // as long as regex gets a successful match
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
                // calls the method to multiply and divide and stores return in a list
                List<string> needsAddSubtract = calculateMD(userProblem);
                // calls the method to add and subtract and stores the response as a float
                float answer = calculateAS(needsAddSubtract);
                // provides the answer
                Console.WriteLine("The answer to {0} is {1:#,#0.#####}.", question, answer);
                // starts the program over
                return true;
            }
            // Writes an invalid entry error to console
            else
            {
                Console.WriteLine("Your entry is invalid. Please only enter numbers and operators. Also, don't begin or trail your equation with an operator. :)");
                // starts the program over
                return true;
            }
        }
        // calculates multiplication and division
        public static List<string> calculateMD(List<string> userProblem)
        {
            List<string> doubleProblem = new List<string>();

            // int for how often the switch hits * and /
            int countHappens = 1;
            // loop through each match in the userProblem list
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
        // method to calculate adding and subtracting
        public static float calculateAS(List<string> needsAddSubtract)
        {
            // places the first list item as the answer to add/subtract from
            float answer = float.Parse(needsAddSubtract[0], CultureInfo.InvariantCulture.NumberFormat);
            // loops through list of numbers that need to be added or subtracted still
            for (int i = 0; i < needsAddSubtract.Count; i++)
            {
                // if the next list item is a + add the following list item to answer
                if (needsAddSubtract[i] == "+")
                {
                    answer += float.Parse(needsAddSubtract[i + 1], CultureInfo.InvariantCulture.NumberFormat);
                }
                // if the next list item is a - subtract the following list item from the answer
                else if (needsAddSubtract[i] == "-")
                {
                    answer -= float.Parse(needsAddSubtract[i + 1], CultureInfo.InvariantCulture.NumberFormat);
                }

            } // end of for loop for switch

            return answer;
        } // end of calculatAS method
    }
}
