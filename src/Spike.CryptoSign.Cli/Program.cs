using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PowerArgs;

namespace Spike.CryptoSign.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<CliProgram>(args);
        }
    }

    internal class CliProgram
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Adds the two operands")]
        public void Test(TestArgs args)
        {
            var fullKeyBytes = File.ReadAllBytes(args.FullKey);
            var fullKey = new X509Certificate2(fullKeyBytes);

            var publicKeyBytes = File.ReadAllBytes(args.PublicKey);
            var publicKey = new X509Certificate2(publicKeyBytes);

            Console.WriteLine($"text: {args.Text}");
            if (fullKey == null) throw new ArgumentNullException(nameof(fullKey));
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));

            var encoding = new UnicodeEncoding();
            var text = args.Text;
            var payload = encoding.GetBytes(text);
            var data = new MemoryStream(payload);
            var signature = fullKey.Sign(data);
            var signatureString = System.Convert.ToBase64String(signature); 
            var verifiedWithFullKey = fullKey.Verify(data, signature);
            var verifiedWithPublicKey = publicKey.Verify(data, signature);

            Console.WriteLine(
                $"verifiedWithFullKey: {verifiedWithFullKey},verifiedWithPublicKey: {verifiedWithPublicKey}");
            Console.WriteLine($"signatureString: {signatureString}");
        }
    }

    internal class TestArgs
    {
        [ArgRequired, ArgDescription("The text to sign"), ArgPosition(0)]
        public string Text { get; set; }
        [ArgRequired, ArgDescription("The the path to the full key (pfx)"), ArgPosition(1)]
        public string FullKey { get; set; }
        [ArgRequired, ArgDescription("The path tot the public key (cer)"), ArgPosition(2)]
        public string PublicKey { get; set; }
    }
}
