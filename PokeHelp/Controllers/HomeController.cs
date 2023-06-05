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

                    var heightInMeters = height * 0.1; // Convert to meters
                    var weightInKilograms = weight * 0.1; // Convert to kilograms

                    var heightInFeet = heightInMeters * 3.281; // Convert to feet
                    var weightInPounds = weightInKilograms * 2.205; // Convert to pounds

                    var stats = new
                    {
                        metric = new { height = $"{heightInMeters:F2} m", weight = $"{weightInKilograms:F2} kg" },
                        imperial = new { height = $"{heightInFeet:F2} ft", weight = $"{weightInPounds:F2} lbs" }
                    };

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