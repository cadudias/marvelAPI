using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarvelApi.Controllers
{
    public class HomeController : Controller
    {
        public PersonagemService service;

        public HomeController(PersonagemService service) => this.service = service;

        public async Task<IActionResult> Index() => View(await service.ObterPersonagemAsync("Captain America"));
    }
}
