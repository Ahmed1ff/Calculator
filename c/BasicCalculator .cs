
using c;
using System;
using System.Collections.Generic;




    

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

            double result = EvaluateExpression(input);

            LastAnswer = result;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Result: {result}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            PrintError($"Error: {ex.Message}");
        }
    }






    private double EvaluateExpression(string input)
    {
        try
        {
            input = input.Replace(" ", ""); // Remove spaces
            Stack<double> numbers = new Stack<double>();
            Stack<char> operators = new Stack<char>();

            int i = 0;
            while (i < input.Length)
            {
                char currentChar = input[i];

                if (char.IsDigit(currentChar) || currentChar == '.') // Handle numbers
                {
                    string number = "";
                    while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                    {
                        number += input[i];
                        i++;
                    }
                    numbers.Push(double.Parse(number));
                    continue;
                }
                else if (currentChar == '(') // Handle opening parenthesis
                {
                    operators.Push(currentChar);
                }
                else if (currentChar == ')') // Handle closing parenthesis
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        ComputeTop(numbers, operators);
                    }
                    if (operators.Count == 0 || operators.Peek() != '(')
                        throw new Exception("Mismatched parentheses.");
                    operators.Pop();

                    // Add implicit multiplication if ')' is followed by a number or '('
                    if (i + 1 < input.Length && (char.IsDigit(input[i + 1]) || input[i + 1] == '('))
                    {
                        operators.Push('*');
                    }
                }
                else if ("+-*/%".Contains(currentChar)) // Handle operators
                {
                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(currentChar))
                    {
                        ComputeTop(numbers, operators);
                    }
                    operators.Push(currentChar);
                }
                else
                {
                    throw new Exception($"Unexpected character: {currentChar}");
                }

                i++;
            }

            while (operators.Count > 0) // Process remaining operators
            {
                ComputeTop(numbers, operators);
            }

            if (numbers.Count != 1)
                throw new Exception("Invalid expression format.");

            return numbers.Pop();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing expression: {ex.Message}");
        }
    }


    private int Precedence(char op)
    {
        return op switch
        {
            '+' or '-' => 1,
            '*' or '/' or '%' => 2,
            _ => 0
        };
    }

    private void ComputeTop(Stack<double> numbers, Stack<char> operators)
    {
        if (numbers.Count < 2 || operators.Count == 0)
            throw new Exception("Invalid expression format.");

        double b = numbers.Pop();
        double a = numbers.Pop();
        char op = operators.Pop();

        double result = op switch
        {
            '+' => a + b,
            '-' => a - b,
            '*' => a * b,
            '/' => b == 0 ? throw new DivideByZeroException("Cannot divide by zero.") : a / b,
            '%' => b == 0 ? throw new DivideByZeroException("Cannot modulo by zero.") : a % b,
            _ => throw new InvalidOperationException($"Unknown operator: {op}")
        };

        numbers.Push(result);
    }










    private void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }



}

