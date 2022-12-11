using FaceitRankChecker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FaceitRankChecker.Controllers
{
    public class FaceitAPI : Controller
    {
        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://open.faceit.com/data/v4");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 0a486c04-b9c1-406f-b5c2-087d4458a86d");
                //HTTP GET
                var responseTask = client.GetAsync("players?nickname=Marulqta&game=csgo");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();
                }
            }
            return Index();
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
                Console.WriteLine(content);
                
               
            }
            return View();
        }

        
    }
}
