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
        public async Task<IActionResult> FaceitCallbackAsync(string code)
        {
            string auth = "e9e58299-32c8-425d-9d12-0b61f4955774:ggmW0rmIgTXbZakY1wMU0jcRiquBYpPP9Vu1OzLb";
            string encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
            

            using  (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.faceit.com");
                
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedAuth}");
                var data = new Dictionary<string, string>
                {
                    {"code", code},
                    {"grant_type", "authorization_code"}
                };

                var url = "/auth/v1/oauth/token";
                var res = await client.PostAsync(url,new FormUrlEncodedContent(data));
                var content = await res.Content.ReadAsStringAsync();
                

                dynamic deserializedResponse = JsonConvert.DeserializeObject(content);


                var stream = deserializedResponse.id_token;
               var handler = new JwtSecurityTokenHandler();
                Console.WriteLine(stream);
                Console.WriteLine("-------------------");
               var jsonToken = handler.ReadToken(stream.ToString());
                var tokenS = jsonToken as JwtSecurityToken;
                
                Console.WriteLine(tokenS.Claims.First(c => c.Type == "email").Value);

            }
            return View();
        }

        
    }
}
