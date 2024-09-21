namespace Calculator
{
    public class ScientificOperations : ICalculatorOperation
    {
        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Scientific Operations:");
            Console.WriteLine("1. Square Root");
            Console.WriteLine("2. Percentage");
            Console.WriteLine("3. Factorial");
            Console.WriteLine("4. Exponential (e^x)");
            Console.WriteLine("5. Complex Number (a + bi)");

            string operation = Console.ReadLine();

            try
            {
                switch (operation)
                {
                    case "1":
                        double num = InputHelper.ReadDouble("Enter number: ");
                        if (num < 0) throw new ArgumentException("Error: Square root is not defined for negative numbers.");
                        Console.WriteLine($"Square Root: {Math.Sqrt(num):F2}");
                        break;
                    case "2":
                        double total = InputHelper.ReadDouble("Enter total: ");
                        double percentage = InputHelper.ReadDouble("Enter percentage: ");
                        Console.WriteLine($"Percentage: {total * (percentage / 100):F2}");
                        break;
                    case "3":
                        int n = (int)InputHelper.ReadDouble("Enter a non-negative integer for factorial: ");
                        if (n < 0) Console.WriteLine("Error: Factorial is not defined for negative numbers.");
                        else Console.WriteLine($"Factorial of {n}: {Factorial(n)}");
                        break;
                    case "4":
                        double expNum = InputHelper.ReadDouble("Enter exponent: ");
                        Console.WriteLine($"e^{expNum}: {Math.Exp(expNum):F2}");
                        break;
                    case "5":
                        double realPart = InputHelper.ReadDouble("Enter real part (a): ");
                        double imaginaryPart = InputHelper.ReadDouble("Enter imaginary part (b): ");
                        Console.WriteLine($"Complex Number: {realPart} + {imaginaryPart}i");
                        break;
                    default:
                        Console.WriteLine("Invalid operation.");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        private long Factorial(int n)
        {
            long result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }


}
