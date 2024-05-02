namespace _2Aplikacija
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new SignatureServer();
            
            server.StartServer();
        }
    }
}
