using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace PokemonStats.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Get(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var pokemonData = JsonDocument.Parse(json).RootElement;
                    var height = pokemonData.GetProperty("height").GetInt32();
                    var weight = pokemonData.GetProperty("weight").GetInt32();

                    var stats = new { height, weight };
                    return Json(stats);
                }
                else
                {
                    return Json("Failed to retrieve Pokémon data.");
                }
            }
        }
    }
}
