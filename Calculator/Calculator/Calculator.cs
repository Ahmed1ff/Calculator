namespace Calculator
{
    public class Calculator
    {
        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose Calculator Type:");
                Console.WriteLine("1. Basic");
                Console.WriteLine("2. Advanced");
                Console.WriteLine("3. Scientific");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                ICalculatorOperation operation = choice switch
                {
                    "1" => new BasicOperations(),
                    "2" => new AdvancedOperations(),
                    "3" => new ScientificOperations(),
                    "4" => null,
                    _ => throw new InvalidOperationException("Invalid option. Please try again.")
                };

                if (operation == null)
                {
                    return;
                }

                operation.Execute();
            }
        }
    }


}
