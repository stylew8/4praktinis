using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _3Aplikacija
{
    public class SignatureValidator
    {
        private int port = 11001;

        public void StartListening()
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Waiting for connection...");

            while (true)
            {
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var parts = receivedData.Split('|');
                    if (parts.Length == 3)
                    {
                        string message = parts[0];
                        string signature = parts[1];
                        string publicKeyXml = parts[2];

                        bool isValid = VerifySignature(publicKeyXml, message, signature);
                        Console.WriteLine($"Received message: {message}");
                        Console.WriteLine($"Signature valid: {isValid}");
                    }
                }
            }
        }

        private bool VerifySignature(string publicKeyXml, string data, string signatureBase64)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKeyXml);
                var bytes = Encoding.UTF8.GetBytes(data);
                var signature = Convert.FromBase64String(signatureBase64);
                return rsa.VerifyData(bytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
