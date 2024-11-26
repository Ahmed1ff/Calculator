using c;


public class BasicCalculator : ICalculator
{

    public double LastAnswer { get; set; } = 0;

    public void DisplayOperations()
    {
        Console.WriteLine("Basic Operations  :");
        Console.WriteLine(". Addition (+)");
        Console.WriteLine(". Subtraction (-)");
        Console.WriteLine(". Multiplication (*)");
        Console.WriteLine(". Division (/)");
        Console.WriteLine(". Modulo (%)");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nYou can enter any basic mathematical operation with any mumber parameters (e.x., 5+5+5*5/5):");
        Console.WriteLine("Use 'ANS' or 'ans' to reference the last result (e.g., ANS+5 or ans+5).");
        Console.ResetColor();
    }


    public void Calculate(string input)
    {
        try
        {
            input = input.ToUpper().Replace("ANS", LastAnswer.ToString());

            
            if (!IsValidExpression(input))
            {
                throw new FormatException("Invalid input. Please enter a valid mathematical expression.");
            }

            double result = EvaluateExpression(input);

            LastAnswer = result;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Result: {result}");
            Console.ResetColor();
        }
        catch (DivideByZeroException ex)
        {
            ErrorHandler.HandleError(ex, "Cannot divide by zero. Please check your input.");
        }
        catch (FormatException ex)
        {
            ErrorHandler.HandleError(ex, "Invalid input format. Please check your input.");
        }
        catch (Exception ex)
        {
            ErrorHandler.HandleError(ex);
        }
    }

    private double EvaluateExpression(string input)
    {
        try
        {
            input = input.Replace(" ", "");

            while (input.Contains("("))
            {
                int openIndex = input.LastIndexOf('('); 
                int closeIndex = input.IndexOf(')', openIndex); 

                if (closeIndex == -1)
                    throw new FormatException("Unmatched parentheses.");

                string innerExpression = input.Substring(openIndex + 1, closeIndex - openIndex - 1);

                double innerResult = EvaluateExpression(innerExpression);

                input = input.Substring(0, openIndex) + innerResult.ToString() + input.Substring(closeIndex + 1);
            }

            return EvaluateSimpleExpression(input);
        }
        catch
        {
            throw new Exception("Invalid input format.");
        }
    }


    private double EvaluateSimpleExpression(string input)
    {
        List<char> operators = new List<char>();
        List<double> numbers = new List<double>();
        string currentNumber = "";

        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];

            if (ch == '-' && (i == 0 || "+-*/%".Contains(input[i - 1])))
            {
                currentNumber += ch; 
            }
            else if (char.IsDigit(ch) || ch == '.')
            {
                currentNumber += ch; 
            }
            else if ("+-*/%".Contains(ch))
            {
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    numbers.Add(double.Parse(currentNumber));
                    currentNumber = "";
                }
                operators.Add(ch); 
            }
        }

        if (!string.IsNullOrEmpty(currentNumber))
            numbers.Add(double.Parse(currentNumber));

        for (int i = 0; i < operators.Count; i++)
        {
            if (operators[i] == '*' || operators[i] == '/' || operators[i] == '%')
            {
                if ((operators[i] == '/' || operators[i] == '%') && numbers[i + 1] == 0)
                    throw new DivideByZeroException("Cannot divide by zero.");

                double result = operators[i] switch
                {
                    '*' => numbers[i] * numbers[i + 1],
                    '/' => numbers[i] / numbers[i + 1],
                    '%' => numbers[i] % numbers[i + 1],
                    _ => throw new InvalidOperationException("Unknown operator")
                };

                numbers[i] = result;
                numbers.RemoveAt(i + 1);
                operators.RemoveAt(i--);
            }
        }

        for (int i = 0; i < operators.Count; i++)
        {
            double result = operators[i] switch
            {
                '+' => numbers[i] + numbers[i + 1],
                '-' => numbers[i] - numbers[i + 1],
                _ => throw new InvalidOperationException("Unknown operator")
            };

            numbers[i] = result;
            numbers.RemoveAt(i + 1);
            operators.RemoveAt(i--);
        }

        return numbers[0];
    }



    private bool IsValidExpression(string input)
    {
        return !string.IsNullOrWhiteSpace(input) &&
               System.Text.RegularExpressions.Regex.IsMatch(input, @"^[\d+\-*/%().\s]+$");
    }




}
