namespace Calculator
{
    public class AdvancedOperations : ICalculatorOperation
    {
        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Advanced Operations:");
            Console.WriteLine("1. Sine (sin)");
            Console.WriteLine("2. Cosine (cos)");
            Console.WriteLine("3. Tangent (tan)");
            Console.WriteLine("4. Logarithm (log)");
            Console.WriteLine("5. Natural Logarithm (ln)");

            string operation = Console.ReadLine();
            double num;

            switch (operation)
            {
                case "1":
                    num = InputHelper.ReadDouble("Enter angle in degrees: ");
                    Console.WriteLine($"sin({num}): {Math.Sin(num * (Math.PI / 180)):F2}");
                    break;
                case "2":
                    num = InputHelper.ReadDouble("Enter angle in degrees: ");
                    Console.WriteLine($"cos({num}): {Math.Cos(num * (Math.PI / 180)):F2}");
                    break;
                case "3":
                    num = InputHelper.ReadDouble("Enter angle in degrees: ");
                    Console.WriteLine($"tan({num}): {Math.Tan(num * (Math.PI / 180)):F2}");
                    break;
                case "4":
                    num = InputHelper.ReadDouble("Enter number: ");
                    if (num <= 0) throw new ArgumentException("Error: Logarithm is not defined for non-positive numbers.");
                    Console.WriteLine($"log({num}): {Math.Log10(num):F2}");
                    break;
                case "5":
                    num = InputHelper.ReadDouble("Enter number: ");
                    if (num <= 0) throw new ArgumentException("Error: Natural logarithm is not defined for non-positive numbers.");
                    Console.WriteLine($"ln({num}): {Math.Log(num):F2}");
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }

            Console.ReadLine();
        }
    }



}
