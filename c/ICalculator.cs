namespace c
{
    public interface ICalculator
    {
        double LastAnswer { get; set; }
        void DisplayOperations();
        void Calculate(string input);
    }
}