namespace _3Aplikacija
{
    internal class Program
    {
        static void Main()
        {
            var validator = new SignatureValidator();
            validator.StartListening();
        }
    }
}
