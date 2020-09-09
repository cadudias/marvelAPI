using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Api
{
    public class PersonagemService
    {
        private readonly IHttpClient client;
        private readonly Configuracao config;
        readonly Relogio relogio;

        public PersonagemService(IHttpClient client, Configuracao config, Relogio relogio) => 
            (this.client, this.config, this.relogio) = (client, config, relogio);

        public async Task<Personagem> ObterPersonagemAsync(string nome)
        {
            var url = new Uri(GerarUrl(nome));
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new Personagem();

            string conteudo = response.Content.ReadAsStringAsync().Result;

            dynamic resultado = JsonConvert.DeserializeObject(conteudo);

            var personagem = new Personagem
            {
                Nome = resultado.data.results[0].name,
                Descricao = resultado.data.results[0].description,
                UrlImagem = resultado.data.results[0].thumbnail.path + "." + resultado.data.results[0].thumbnail.extension,
                UrlWiki = resultado.data.results[0].urls[1].url
            };

            return personagem;
        }

        private string GerarUrl(string nome)
        {
            string ts = relogio.ObterStringTicks();
            string publicKey = config.PublicKey;
            string hash = GerarHash(ts, publicKey, config.PrivateKey);

            return config.Uri +
            $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
            $"name={Uri.EscapeUriString(nome)}";
        }

        private string GerarHash(string ts, string publicKey, string privateKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);
            
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
        }
    }
}
