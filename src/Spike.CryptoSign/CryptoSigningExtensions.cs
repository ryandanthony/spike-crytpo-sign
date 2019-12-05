using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Spike.CryptoSign
{
    public static class CryptoSigningExtensions
    {
        public static byte[] Sign(this X509Certificate2 certificate, byte[] payload)
        {
            var key = certificate.GetRSAPrivateKey();
            if (key == null)
            {
                throw new Exception("Invalid Certificate");
            }

            var sha = new SHA512Managed();
            var hash = sha.ComputeHash(payload);

            // Sign the hash
            return key.SignHash(hash, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }

        public static bool Verify(this X509Certificate2 certificate, byte[] payload, byte[] signature)
        {
            var key = certificate.GetRSAPublicKey();
            if (key == null)
            {
                throw new Exception("Invalid Certificate");
            }

            var sha = new SHA512Managed();
            var hash = sha.ComputeHash(payload);
            return key.VerifyHash(hash, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }
    }
}
