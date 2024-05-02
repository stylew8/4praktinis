using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _1Aplikacija
{
    public class DigitalSignatureSender
    {
        private RSA rsa;
        private string serverIp = "127.0.0.1";
        private int serverPort = 11000;

        public DigitalSignatureSender()
        {
            rsa = RSA.Create(2048);
        }

        public string GetPublicKeyXml()
        {
            return rsa.ToXmlString(false);
        }

        public byte[] SignData(string data)
        {
            var encoder = new UTF8Encoding();
            byte[] bytes = encoder.GetBytes(data);
            return rsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        public void SendData(string message)
        {
            byte[] signature = SignData(message);
            string publicKeyXml = GetPublicKeyXml();
            string payload = $"{message}|{Convert.ToBase64String(signature)}|{publicKeyXml}";

            using (var client = new TcpClient(serverIp, serverPort))
            using (var stream = client.GetStream())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(payload);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
