namespace Calculator
{
    public class BasicOperations : ICalculatorOperation
    {
        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Basic Operations:");
            Console.WriteLine("1. Addition (+)");
            Console.WriteLine("2. Subtraction (-)");
            Console.WriteLine("3. Multiplication (*)");
            Console.WriteLine("4. Division (/)");
            Console.WriteLine("5. Modulo (%)");

            string operation = Console.ReadLine();
            double num1 = InputHelper.ReadDouble("Enter first number: ");
            double num2 = InputHelper.ReadDouble("Enter second number: ");

            try
            {
                switch (operation)
                {
                    case "1": Console.WriteLine($"Result: {num1 + num2:F2}"); break;
                    case "2": Console.WriteLine($"Result: {num1 - num2:F2}"); break;
                    case "3": Console.WriteLine($"Result: {num1 * num2:F2}"); break;
                    case "4":
                        if (num2 == 0) throw new DivideByZeroException("Error: Division by zero");
                        Console.WriteLine($"Result: {num1 / num2:F2}");
                        break;
                    case "5":
                        if (num2 == 0) throw new DivideByZeroException("Error: Division by zero (for modulo)");
                        Console.WriteLine($"Result: {num1 % num2:F2}");
                        break;
                    default: Console.WriteLine("Invalid operation."); break;
                }
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }


}
