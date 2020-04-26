using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marvel.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagemController : ControllerBase
    {
        public PersonagemService service;

        public PersonagemController(PersonagemService service) => this.service = service;

        [HttpGet]
        public async Task<Personagem> Get()
        {
            return await service.ObterPersonagemAsync("Captain America");
        }   
    }
}