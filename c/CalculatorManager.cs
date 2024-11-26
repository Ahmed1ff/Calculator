using c;

public class CalculatorManager
{
    private readonly Dictionary<string, ICalculator> calculators;

    public CalculatorManager()
    {
        calculators = new Dictionary<string, ICalculator>
        {
            { "1", new BasicCalculator() },
            { "2", new AdvancedCalculator() },
            { "3", new ScientificCalculator() }
        };
    }

    public void Run()
    {
        string errorMessage = string.Empty; 

        while (true)
        {
            try
            {
                Console.Clear();
                UIHelper.DisplayHeader("Calculator Menu");
                Console.WriteLine("1. Basic Calculator");
                Console.WriteLine("2. Advanced Calculator");
                Console.WriteLine("3. Scientific Calculator");
                Console.WriteLine("4. Exit");
                UIHelper.DisplaySeparator();

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    UIHelper.DisplayMessage(errorMessage, ConsoleColor.Red);
                    errorMessage = string.Empty; 
                }

                string choice = UIHelper.PromptInput("Enter your choice (1,2,3,4): ");

                if (choice == "4")
                {
                    UIHelper.DisplayMessage("Exiting the program. Goodbye!", ConsoleColor.Yellow);
                    break;
                }

                if (calculators.ContainsKey(choice))
                {
                    RunCalculator(calculators[choice]);
                }
                else
                {
                    throw new ArgumentException("Invalid choice. Please enter a number between 1 and 4.");
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
            }
        }
    }



    private void RunCalculator(ICalculator calculator)
    {
        while (true)
        {
            try
            {
                Console.Clear();
                calculator.DisplayOperations();
                Console.WriteLine("\nEnter your input (or type 'back' or 'b' to return to the main menu):");
                string input = UIHelper.PromptInput("");

                if (input == "back" || input == "b")
                {
                    UIHelper.DisplayMessage("Returning to the main menu", ConsoleColor.Cyan);
                    break;
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new ArgumentException("Input cannot be empty or whitespace.");
                }

                calculator.Calculate(input);
                Console.WriteLine("\nPress any key to continue...");
           //     Console.ReadKey();
            }
            catch (FormatException ex)
            {
                ErrorHandler.HandleError(ex, "Invalid format. Please check your input.");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex, "An unexpected error occurred. Please try again.");
            }

            Console.ReadKey();
        }
    }

}
