using System;

namespace Marvel.Api
{
    // todo pegar configs sem usar essa classe
    public class Configuracao
    {
        public Configuracao(Uri uri, string publicKey, string privateKey)
        {
            Uri = uri;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public Uri Uri { get; }
        public string PublicKey { get; }
        public string PrivateKey { get; }
    }
}
