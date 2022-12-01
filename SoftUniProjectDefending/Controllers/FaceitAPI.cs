using FaceitRankChecker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

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
            return readTask();
        }
    }
}
