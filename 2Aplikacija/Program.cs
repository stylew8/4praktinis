using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _2Aplikacija
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Loopback;
                int port = 11111;
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

                using (Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    listener.Bind(ipEndPoint);
                    listener.Listen(10);

                    Console.WriteLine("Laukiama pirmo kliento prisijungimo...");
                    using (Socket handlerFirst = listener.Accept())
                    {
                        Console.WriteLine("Pirmas klientas prisijunge.");
                        var buffer = new byte[4096];
                        int bytesReceived = handlerFirst.Receive(buffer);
                        var receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                        Console.WriteLine($"Gauta is pirmo kliento: {receivedData}");

                        Console.WriteLine("Ar norite pakeisti parasa? (y/n)");
                        var response = Console.ReadLine().ToLower();
                        if (response == "y")
                        {
                            Console.WriteLine("Iveskite teksta:");
                            var userInput = Console.ReadLine();
                            var parts = receivedData.Split(':');
                            var message = parts[0];
                            var modifiedSignature = Encoding.UTF8.GetBytes(userInput ?? "");
                            var publicKeyXml = parts[2];
                            receivedData = $"{message}:{Convert.ToBase64String(modifiedSignature)}:{publicKeyXml}";
                            buffer = Encoding.UTF8.GetBytes(receivedData);
                            bytesReceived = buffer.Length;
                        }

                        Console.WriteLine("Laukiama antro kliento prisijungimo...");
                        using (Socket handlerSecond = listener.Accept())
                        {
                            Console.WriteLine("Antras klientas prisijunges. Siunciami duomenys...");
                            handlerSecond.Send(buffer, bytesReceived, SocketFlags.None);
                        }

                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
