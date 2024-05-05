using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _1Aplikacija
{
    class Program
    {
        static async Task Main()
        {
            while (true)
            {
                var sign = new DigitalSignature();
                try
                {
                    var keys = sign.AssignNewKey();

                    IPAddress ipAddress = IPAddress.Loopback;
                    int port = 11111;
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

                    using (Socket client = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                    {
                        client.Connect(ipEndPoint);

                        Console.WriteLine("Iveskite Teksta");
                        string message = Console.ReadLine();
                        var signature = sign.SignData(message);
                        var publicKeyXml = sign.ExportPublicKey();
                        var fullMessage = $"{message}:{Convert.ToBase64String(signature)}:{publicKeyXml}";
                        var messageBytes = Encoding.UTF8.GetBytes(fullMessage);

                        client.Send(messageBytes);
                        Console.WriteLine("Zinute, parasas, ir viesasis raktas issiusti.");

                        Console.ReadKey();

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
