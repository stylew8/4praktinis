using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _2Aplikacija
{
    public class SignatureServer
    {
        private int port = 11000;

        public void StartServer()
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("Received: " + receivedData);
                    // Разделение данных
                    var parts = receivedData.Split('|');
                    string message = parts[0];
                    string signature = parts[1];
                    string publicKeyXml = parts[2];

                    // Модификация подписи (пример модификации, не рекомендуется на практике)
                    string modifiedSignature = Convert.ToBase64String(Encoding.UTF8.GetBytes(signature + "modified"));

                    // Передача данных в третье приложение
                    SendDataToAnotherApplication(message, modifiedSignature, publicKeyXml);
                }
            }
        }

        private void SendDataToAnotherApplication(string message, string modifiedSignature, string publicKeyXml)
        {
            // Адрес и порт, на котором слушает третье приложение
            string clientIp = "127.0.0.1";
            int clientPort = 11001;

            try
            {
                using (var client = new TcpClient(clientIp, clientPort))
                using (var stream = client.GetStream())
                {
                    // Формирование строки для отправки
                    string payload = $"{message}|{modifiedSignature}|{publicKeyXml}";
                    byte[] buffer = Encoding.UTF8.GetBytes(payload);

                    // Отправка данных
                    stream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("Data sent to the third application.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data: {e.Message}");
            }
        }
    }

}
