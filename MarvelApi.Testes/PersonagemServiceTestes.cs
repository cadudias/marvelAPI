using NUnit.Framework;
using FakeItEasy;
using Autofac.Extras.FakeItEasy;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using Bogus;

namespace MarvelApi.Testes
{
    public class Tests
    {
        private PersonagemService service;
        private AutoFake AutoFake;
        private IConfiguration config;
        public Relogio relogio;
        private string nome;
        private string url;

        protected static Faker Faker { get; } = new Faker();

        [SetUp]
        public void Setup()
        {
            AutoFake = new AutoFake();

            var configurationParams = new Dictionary<string, string>
            {
                { "MarvelComicsAPI:BaseURL", "http://gateway.marvel.com/v1/public/" },
                { "MarvelComicsAPI:PublicKey", "f72ecc981b3848b2d7ae9973d0454131" },
                { "MarvelComicsAPI:PrivateKey", "8614298176bdfa2ecff7a712cfdcb6e73bf1d4f9" },
            };

            config = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationParams)
                .Build();

            AutoFake.Provide(config);

            relogio = A.Fake<Relogio>();
            A.CallTo(() => relogio.DataAtual).Returns(DateTime.Now);
            A.CallTo(() => relogio.ObterStringTicks()).Returns(DateTime.Now.Ticks.ToString());
     
            AutoFake.Provide(relogio);

            // o GetAsync precisa de uma resposta mockada do persogame
            // pra isso preciso cirar uma url no teste e mandar ela pro controller
            string ts = relogio.ObterStringTicks();
            string publicKey = config.GetSection("MarvelComicsAPI:PublicKey").Value;
            string hash = GerarHash(ts, publicKey, config.GetSection("MarvelComicsAPI:PrivateKey").Value);

            nome = "Captain America";

            url = config.GetSection("MarvelComicsAPI:BaseURL").Value +
            $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
            $"name={Uri.EscapeUriString(nome)}";

            // temos que saber o objeto de retorno da api pra poder mockar o retorno do objeto fake na implementacao concreta
            var conteudo = new StringContent(JsonSerializer.Serialize(
               new
               {
                   data = new
                   {
                       results = new List<dynamic> {
                        new {
                            name = nome,
                            description = Faker.Random.Words(20),
                            thumbnail = new { path = Faker.Internet.Url(), extension = Faker.Random.String(3) },
                            urls = new List<dynamic> { new { url = Faker.Internet.Url() }, new { url = Faker.Internet.Url() } }
                        }
                       }
                   }
               }
            ));

            // a resposta vem como um objeto serializado
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK) { Content = conteudo };
            
            A.CallTo(() => AutoFake.Resolve<IHttpClient>().GetAsync(new Uri(url))).Returns(response);

            service = AutoFake.Resolve<PersonagemService>();
        }

        [Test]
        public async Task DeveObterPersonagem()
        {
            var resultado = await service.ObterPersonagemAsync(nome);

            var personagemEsperado = new Personagem() {
                Nome = nome
            };

            // pra testar objetos completos temos que usar o BeEquivalentTo ja que pro compilador
            // verificar se 2 objetos sao iguais nunca vai funcionar ja que são posições diferentes de memoria
            // o que precisamos testar sao os valores desses objetos
            resultado.Should().BeEquivalentTo(personagemEsperado, opt => opt
            .Excluding(x => x.UrlImagem).Excluding(x => x.UrlWiki).Excluding(x => x.Descricao));
        }

        [Test]
        public async Task NaoDeveObterPersonagemSeStatusCodeForDiferenteDeSucesso()
        {
            // podemos enviar status code diferentes usando o contrutor do HttpResponseMessage
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) { };

            A.CallTo(() => AutoFake.Resolve<IHttpClient>().GetAsync(new Uri(url))).Returns(response);

            var resultado = await service.ObterPersonagemAsync(nome);

            var personagemEsperado = new Personagem();
            resultado.Should().BeEquivalentTo(personagemEsperado);
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