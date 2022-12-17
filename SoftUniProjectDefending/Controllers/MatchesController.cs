using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FaceitRankChecker.Controllers
{
    public class MatchesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            dynamic deserializedResponse = "";
            using (var client = new HttpClient())
            {
                string auth = "8a136896-6859-4593-ba6a-03b12a5b91ba";
                client.BaseAddress = new Uri("https://open.faceit.com");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth}");
                //HTTP GET

                var url = $"/data/v4/matches/1-c5a00694-4fbc-4f23-a47a-a0cd4c38bc66";
                var res = await client.GetAsync(url);
                var content = await res.Content.ReadAsStringAsync();

                deserializedResponse = JsonConvert.DeserializeObject(content);


                ViewData["competitionName"] = deserializedResponse.competition_name;

                ViewData["player1"] = deserializedResponse.nickname = "Marulqta";
                
                //ViewData["player2"]
                //ViewData["player3"] 
                //ViewData["player4"] 
                //ViewData["player5"] 
                //ViewData["player6"] 
                //ViewData["player7"] 
                //ViewData["player8"] 
                //ViewData["player9"] 
                //ViewData["player10"]
            }

            return View();
        }

        
    }
    
}

