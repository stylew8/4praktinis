using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _3Aplikacija
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                var sign = new SignatureValidator();

                IPAddress ipAddress = IPAddress.Loopback;
                int port = 11111;
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

                Console.WriteLine("Laukiama antro kliento prisijungimo...");
                using (Socket client = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    Console.WriteLine("Antras klientas prisijunge.");

                    client.Connect(ipEndPoint);

                    var buffer = new byte[4096];
                    int bytesReceived = client.Receive(buffer);
                    var receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    var parts = receivedData.Split(':');
                    var message = parts[0];
                    var signature = Convert.FromBase64String(parts[1]);
                    var publicKeyXml = parts[2];

                    var isValid = sign.VerifySignature(message, signature, publicKeyXml);
                    Console.WriteLine($"Gauta zinute: \"{message}\", ar parasas validus: {isValid}");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
