using FaceitRankChecker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Web.Helpers;

namespace FaceitRankChecker.Controllers
{
    public class FaceitAPI : Controller
    {
        public async Task<IActionResult> Index(string nickname = "Marulqta")
        {
            dynamic deserializedResponse = "";
            using (var client = new HttpClient())
            {
                string auth = "8a136896-6859-4593-ba6a-03b12a5b91ba";
                client.BaseAddress = new Uri("https://open.faceit.com");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth}");
                //HTTP GET

                var url = $"/data/v4/players?nickname={nickname}&game=csgo";
                var res = await client.GetAsync(url);
                var content = await res.Content.ReadAsStringAsync();

                deserializedResponse = JsonConvert.DeserializeObject(content);
                Console.WriteLine(content);
            }
            ViewData["nickname"] = deserializedResponse.nickname;
            ViewData["elo"] = deserializedResponse.games["csgo"].faceit_elo;
            return View();

        }
                
    }
}
