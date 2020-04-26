using Autofac.Extras.FakeItEasy;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using Marvel.Teste.Integracao;
using MarvelApi.Controllers;
using MarvelApi.Testes.Compartilhado;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarvelApi.Teste.Integracao
{
    public class PersonagemControllerTestes
    {
        private HomeController controller;
        private PersonagemService service;
        private AutoFake AutoFake;
        private Relogio relogio;
        private Configuracao configuration;
        private Faker Faker;
        public HttpClient Client { get; private set; } = Setup.Client;

        private readonly CustomWebApplicationFactory<Startup>
         _factory;


        //public PersonagemControllerTestes(CustomWebApplicationFactory<Startup> factory)
        //{
        //    _factory = factory;
        //}

        [SetUp]
        public void SetupTestes()
        {
            AutoFake = new AutoFake();

            Faker = new Faker();

            // só com resolve o fakeItEasy ja cria fakes pras interfaces por padrao
            // só que outras classes vai usar a implementacao concreta e nao é isso que queremos num teste

            // injetar fakes com comportamento - pra criar fakes com comportamentos vamos atribuir comportamentos a eles e 
            // usar o provide pra injetar a classe fake no construtor
            // pra configurar comportamentos pro nosso fake precisamos usar o A.CallTo e o Provide
            //relogio = A.Fake<Relogio>();
            //var ticks = DateTime.Now.AddDays(-1).Ticks.ToString();
            //A.CallTo(() => relogio.ObterStringTicks()).Returns(ticks);
            //AutoFake.Provide(relogio);

            // passando os valores corretos pro config
            configuration = new Configuracao(new Uri(Faker.Internet.Url()), Faker.Random.Word(), Faker.Random.Word());
            AutoFake.Provide(configuration);

            var httpClientFake = A.Fake<IHttpClient>();

            A.CallTo(() => httpClientFake.GetAsync(A<Uri>._))
                                .Returns(new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new StringContent(JsonSerializer.Serialize(new
                                    {
                                        data = new
                                        {
                                            results = new List<dynamic> {
                                            new {
                                                name = "Captain America",
                                                description = Faker.Random.Words(20),
                                                thumbnail = new { path = Faker.Internet.Url(), extension = Faker.Random.String(3) },
                                                urls = new List<dynamic> { new { url = Faker.Internet.Url() }, new { url = Faker.Internet.Url() } }
                                            }
                                           }
                                        }
                                    }))
                                });

            FakerServiceProvider.Add(httpClientFake);

            //service = A.Fake<PersonagemService>();
            //AutoFake.Provide(service);
            controller = AutoFake.Resolve<HomeController>(); 
        }

        [Test]
        public async Task DeveObterPersonagemPorNome()
        {
            // Arrange
            var client = _factory.CreateClient();

            //var resultado = await Client.GetAsync
            var resultado = client.GetAsync("personagem");

            // passando um nome o metodo deve ObterPersonagem deve retornar com o nome correto
            // esse teste acessa a api real ou não?
            // nos nossos testes de obterProduto de simpress isso nao acontece
            // o teste nao pode depender de funcionamentos externos ao controle dele
            // o que tem que acontecer é que dado o retorno esperado da api e passando os addos de entrada um comportamento x deve acontexer

            // enviando um nome
            // chamar o ObterPersonagem
            // devo criar um mock pro getAsync do client que retorna um status OK e um conteudo com o persoagem esperado

            var personagemEsperado = new Personagem
            {
                Nome = "Captain America"
            };

            // sem mockar o client ele ja vem com status OK
            // outro ponto pra ter em mente é que se quisermos mandar um status messgae diferente 
            // precisamos mockar isso de alguma forma, pra nao poluir os testes podemos usar uma classe
            // de Setup que roda antes de rodarmos os testes
            //var resultado = await service.ObterPersonagemAsync(personagemEsperado.Nome);
            //resultado.Should().BeEquivalentTo(personagemEsperado);
        }
    }
}