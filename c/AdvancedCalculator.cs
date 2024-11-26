using c;
using System.Data;
using System.Text.RegularExpressions;

public class AdvancedCalculator : ICalculator
{
    public double LastAnswer { get; set; } = 0;
    private const double E = Math.E;

    public void DisplayOperations()
    {
        Console.WriteLine("Advanced Operations:");
        Console.WriteLine("1. Sine (sin deg or sin(deg))");
        Console.WriteLine("2. Cosine (cos deg or cos(deg))");
        Console.WriteLine("3. Tangent (tan deg or tan(deg))");
        Console.WriteLine("4. Logarithm (log n or log(n))");
        Console.WriteLine("5. Natural Logarithm (ln n or ln(n))");
        Console.WriteLine("6. Logarithm to Base y (logy(base, value))");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Note: You can include mathematical expressions inside the functions, e.x., sin(45 + 45).");
        Console.WriteLine("Note: You can include mathematical expressions between 2 functions, e.x., sin(45 + 45)+cos(45).");
        Console.WriteLine("Use 'ans' or 'ANS' to refer to the result of the previous operation ex ans +sin45.");
        Console.ResetColor();
    }

    public void Calculate(string input)
    {
        try
        {
            input = input.Trim();
            input = input.Replace("ans", LastAnswer.ToString())
                         .Replace("ANS", LastAnswer.ToString());
            input = NormalizeFunctions(input);

            double result = EvaluateExpression(input);

            LastAnswer = result;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Result: {result}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            ErrorHandler.HandleError(ex, "An error occurred while calculating the expression.");
        }
    }

    private double EvaluateExpression(string expression)
    {
        expression = ReplaceFunctionsWithValues(expression);
        DataTable table = new DataTable();
        object result = table.Compute(expression, null);
        return Convert.ToDouble(result);
    }

    private string NormalizeFunctions(string input)
    {
        string[] functions = { "sin", "cos", "tan", "log", "ln", "logy" };

        input = Regex.Replace(input, @"\be\b", E.ToString(), RegexOptions.IgnoreCase);

        foreach (var function in functions)
        {
            if (function == "logy")
            {
                var regexLogyWithoutParentheses = new Regex($@"\b{function}\s*(\d+)\s*,\s*(\d+|\be\b)");
                input = regexLogyWithoutParentheses.Replace(input, match =>
                {
                    string baseValue = match.Groups[1].Value.Trim();
                    string value = match.Groups[2].Value.Trim();
                    return $"{function}({baseValue},{value})";
                });

                var regexLogyWithParentheses = new Regex($@"\b{function}\s*\(\s*([\d+\-*/().\s]+)\s*,\s*([\d+\-*/().\s]+)\s*\)");
                input = regexLogyWithParentheses.Replace(input, match =>
                {
                    string baseValue = match.Groups[1].Value.Trim();
                    string value = match.Groups[2].Value.Trim();
                    return $"{function}({baseValue},{value})";
                });
            }
            else
            {
                var regexWithoutParentheses = new Regex($@"\b{function}\s*(\d+|\be\b)");
                input = regexWithoutParentheses.Replace(input, match =>
                {
                    string value = match.Groups[1].Value.Trim();
                    return $"{function}({value})";
                });

                var regexWithParentheses = new Regex($@"\b{function}\s*\(([\d+\-*/().\s]+)\)");
                input = regexWithParentheses.Replace(input, match =>
                {
                    return match.Value;
                });
            }
        }

        return Regex.Replace(input, @"\s+", "");
    }

    private string ReplaceFunctionsWithValues(string input)
    {
        string[] functions = { "sin", "cos", "tan", "logy", "log", "ln" };

        foreach (string function in functions)
        {
            while (true)
            {
                int index = input.IndexOf(function, StringComparison.OrdinalIgnoreCase);
                if (index == -1) break;

                int startIndex = index + function.Length;
                if (startIndex >= input.Length || input[startIndex] != '(')
                    throw new InvalidOperationException($"Invalid syntax for '{function}': Missing opening parenthesis.");

                int endIndex = FindClosingParenthesis(input, startIndex);
                if (endIndex == -1)
                    throw new InvalidOperationException($"Invalid syntax for '{function}': Missing closing parenthesis.");

                string inside = input.Substring(startIndex + 1, endIndex - startIndex - 1);

                double value = function switch
                {
                    "sin" => Math.Sin(DegreeToRadian(EvaluateExpression(inside))),
                    "cos" => Math.Cos(DegreeToRadian(EvaluateExpression(inside))),
                    "tan" => Math.Tan(DegreeToRadian(EvaluateExpression(inside))),
                    "logy" => EvaluateLogy(inside),
                    "log" => Math.Log10(EvaluateExpression(inside)),
                    "ln" => Math.Log(EvaluateExpression(inside)),
                    _ => throw new InvalidOperationException($"Unknown function '{function}'.")
                };

                input = input.Substring(0, index) + value + input.Substring(endIndex + 1);
            }
        }

        return input;
    }

    private double EvaluateLogy(string expression)
    {
        string[] parts = expression.Split(',');
        if (parts.Length != 2)
            throw new InvalidOperationException("Logy requires two arguments: base and value.");

        double baseValue = parts[0].Trim().Equals("e", StringComparison.OrdinalIgnoreCase) ? E : EvaluateExpression(parts[0].Trim());
        double value = parts[1].Trim().Equals("e", StringComparison.OrdinalIgnoreCase) ? E : EvaluateExpression(parts[1].Trim());

        if (baseValue <= 0 || baseValue == 1 || value <= 0)
            throw new InvalidOperationException("Invalid arguments for logy.");

        return Math.Log(value, baseValue);
    }

    private int FindClosingParenthesis(string input, int startIndex)
    {
        int count = 0;

        for (int i = startIndex; i < input.Length; i++)
        {
            if (input[i] == '(') count++;
            else if (input[i] == ')') count--;

            if (count == 0) return i;

            if (count < 0)
                throw new InvalidOperationException("Too many closing parentheses.");
        }

        if (count > 0)
            throw new InvalidOperationException("Missing closing parenthesis.");

        return -1;
    }

    private double DegreeToRadian(double degree) => degree * (Math.PI / 180.0);
}
