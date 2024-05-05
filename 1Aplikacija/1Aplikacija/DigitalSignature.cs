using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _1Aplikacija
{
    public class DigitalSignature
    {
        private  RSAParameters _privateKey;
        private  RSAParameters _publicKey;


        public (RSAParameters, RSAParameters) AssignNewKey()
        {
            using (RSA rsa = RSA.Create(2048))
            {
                _privateKey = rsa.ExportParameters(true);
                _publicKey = rsa.ExportParameters(false);
            }

            return (_publicKey, _privateKey);
        }

        public byte[] SignData(string data)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_privateKey);
                var dataBytes = Encoding.UTF8.GetBytes(data);
                return rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public string ExportPublicKey()
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_publicKey);
                return rsa.ToXmlString(false);
            }
        }
    }
}
