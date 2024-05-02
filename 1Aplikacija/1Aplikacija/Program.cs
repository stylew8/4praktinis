namespace _1Aplikacija
{
    class Program
    {
        static async Task Main()
        {
            var sender = new DigitalSignatureSender();
            sender.SendData("Hello, World!");
        }
    }
}
