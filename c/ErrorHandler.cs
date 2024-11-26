

namespace c
{
    public static class ErrorHandler
    {
        public static void HandleError(Exception ex, string userMessage = "An error occurred")
        {
            UIHelper.DisplayHeader("Error", ConsoleColor.DarkRed);  
            UIHelper.DisplayMessage(userMessage, ConsoleColor.DarkRed);
            UIHelper.DisplayMessage($"Details: {ex.Message}", ConsoleColor.DarkRed);
           // Console.WriteLine("\nPress any key to continue");
            //Console.ReadKey();

        }
    }

}
