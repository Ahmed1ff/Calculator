

namespace c
{
   

    public static class UIHelper
    {
        public static void DisplayHeader(string title, ConsoleColor color = ConsoleColor.Green)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(new string('=', 30));
            Console.WriteLine($"        {title}        ");
            Console.WriteLine(new string('=', 30));
            Console.ResetColor();
        }

        public static void DisplayMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void DisplaySeparator(char character = '-', int length = 30)
        {
            Console.WriteLine(new string(character, length));
        }

        public static string PromptInput(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input == null)
            {
                throw new InvalidOperationException("Input cannot be null.");
            }

            return input;
        }

    }

}
