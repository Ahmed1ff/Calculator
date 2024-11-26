using c;
using System.Data;

public class ScientificCalculator : ICalculator
{
    public double LastAnswer { get; set; } = 0;

    public void DisplayOperations()
    {
        Console.WriteLine("Scientific Operations:");
        Console.WriteLine("1. Square Root (sqrt number or sqrt(expression))");
        Console.WriteLine("2. Percentage (n%n or n% or %n or %(expression))");
        Console.WriteLine("3. Factorial (n! or !(expression))");
        Console.WriteLine("4. Exponential (exp number or exp(expression))");
        Console.WriteLine("5. Complex Number (num ± numi or expressions)");
        Console.WriteLine("6. Power (base^exponent or expressions)");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Note: You can include mathematical expressions inside the functions ex:   sqrt(2+2).");
        Console.WriteLine("Note: You can include mathematical expressions between 2 functions, e.x., exp(4)+sqrt(4).");
        Console.WriteLine("Use 'ans' or 'ANS' to refer to the result of the previous operation ex ans +exp5.");
        Console.ResetColor();
    }

    public void Calculate(string input)
    {
        try
        {
            input = input.Trim();
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", "");
            input = input.Replace("ans", LastAnswer.ToString(), StringComparison.OrdinalIgnoreCase);

            if (input.Contains("i"))
            {
                ProcessComplexNumber(input);
            }
            else
            {
                input = ProcessCustomFunctions(input);
                double result = EvaluateExpression(input);
                DisplayResult(result);
                LastAnswer = result;
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.HandleError(ex, "Error occurred during calculation.");
        }
    }




    private string ProcessCustomFunctions(string input)
    {
        while (System.Text.RegularExpressions.Regex.IsMatch(input, @"sqrt\((.*?)\)|sqrt(\d+)"))
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"sqrt\((.*?)\)", match =>
            {
                string expression = match.Groups[1].Value;
                double value = EvaluateExpression(expression);
                if (value < 0)
                    throw new InvalidOperationException("Square root is only defined for non-negative numbers.");
                return Math.Sqrt(value).ToString();
            });

            input = System.Text.RegularExpressions.Regex.Replace(input, @"sqrt(\d+)", match =>
            {
                double value = double.Parse(match.Groups[1].Value);
                if (value < 0)
                    throw new InvalidOperationException("Square root is only defined for non-negative numbers.");
                return Math.Sqrt(value).ToString();
            });
        }

        while (System.Text.RegularExpressions.Regex.IsMatch(input, @"!(\d+|\(.*?\))"))
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"!(\d+|\(.*?\))", match =>
            {
                string expression = match.Groups[1].Value;
                double value = EvaluateExpression(expression);
                if (value < 0 || value != Math.Floor(value))
                    throw new InvalidOperationException("Factorial is only defined for non-negative integers.");
                return Factorial((int)value).ToString();
            });
        }

        while (System.Text.RegularExpressions.Regex.IsMatch(input, @"(\d+|\(.*?\))!"))
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"(\d+|\(.*?\))!", match =>
            {
                string expression = match.Groups[1].Value;
                double value = EvaluateExpression(expression);
                if (value < 0 || value != Math.Floor(value))
                    throw new InvalidOperationException("Factorial is only defined for non-negative integers.");
                return Factorial((int)value).ToString();
            });
        }

        while (System.Text.RegularExpressions.Regex.IsMatch(input, @"(\d+|\(.*?\))\^(\d+|\(.*?\))"))
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"(\d+|\(.*?\))\^(\d+|\(.*?\))", match =>
            {
                string baseExpression = match.Groups[1].Value;
                string exponentExpression = match.Groups[2].Value;

                double baseValue = EvaluateExpression(baseExpression);
                double exponentValue = EvaluateExpression(exponentExpression);

                return Math.Pow(baseValue, exponentValue).ToString();
            });
        }

        while (System.Text.RegularExpressions.Regex.IsMatch(input, @"(\d+|\(.*?\))%"))
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"(\d+|\(.*?\))%", match =>
            {
                string expression = match.Groups[1].Value;
                double value = EvaluateExpression(expression);
                return (value / 100).ToString();
            });
        }

        while (System.Text.RegularExpressions.Regex.IsMatch(input, @"exp\((.*?)\)|exp(\d+)"))
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"exp\((.*?)\)", match =>
            {
                string expression = match.Groups[1].Value;
                double value = EvaluateExpression(expression);
                return Math.Exp(value).ToString();
            });

            input = System.Text.RegularExpressions.Regex.Replace(input, @"exp(\d+)", match =>
            {
                double value = double.Parse(match.Groups[1].Value);
                return Math.Exp(value).ToString();
            });
        }

        return input;
    }






    private void ProcessRealSum(string input)
    {
        try
        {
            var parts = System.Text.RegularExpressions.Regex.Split(input, @"(?=[+-])");

            double realSum = 0;

            foreach (var part in parts)
            {
                string trimmedPart = part.Trim();

                if (string.IsNullOrWhiteSpace(trimmedPart))
                    continue;

                if (double.TryParse(trimmedPart, out double realValue))
                {
                    realSum += realValue;
                }
                else
                {
                    throw new InvalidOperationException($"Invalid real part: {trimmedPart}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Real Sum: {realSum}");
            Console.WriteLine($"Imaginary Sum: 0");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            ErrorHandler.HandleError(ex, "Error processing real numbers.");
        }
    }




    private void ProcessComplexNumber(string input)
    {
        try
        {
            input = input.Replace(" ", "");

            var parts = System.Text.RegularExpressions.Regex.Split(input, @"(?=[+-])");

            double realSum = 0;
            double imaginarySum = 0;

            foreach (var part in parts)
            {
                string trimmedPart = part.Trim();

                if (string.IsNullOrWhiteSpace(trimmedPart))
                    continue;

                if (trimmedPart.EndsWith("i"))
                {
                    string imaginaryPart = trimmedPart.TrimEnd('i');

                    if (string.IsNullOrWhiteSpace(imaginaryPart) || imaginaryPart == "+" || imaginaryPart == "-")
                    {
                        imaginaryPart = imaginaryPart == "-" ? "-1" : "1";
                    }

                    if (double.TryParse(imaginaryPart, out double imaginaryValue))
                    {
                        imaginarySum += imaginaryValue;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid imaginary part: {trimmedPart}");
                    }
                }
                else
                {
                    if (double.TryParse(trimmedPart, out double realValue))
                    {
                        realSum += realValue;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid real part: {trimmedPart}");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Real Sum: {realSum}");
            Console.WriteLine($"Imaginary Sum: {imaginarySum}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            ErrorHandler.HandleError(ex, "Error processing complex number.");
        }
    }





    private double EvaluateExpression(string expression)
    {
        try
        {
            DataTable table = new DataTable();
            var result = table.Compute(expression, null);
            return Convert.ToDouble(result);
        }
        catch
        {
            throw new InvalidOperationException("Invalid mathematical expression.");
        }
    }



    private int Factorial(int n)
    {
        if (n == 0 || n == 1) return 1;
        return n * Factorial(n - 1);
    }


    private void DisplayResult(double result)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Result: {result}");
        Console.ResetColor();
    }
}
