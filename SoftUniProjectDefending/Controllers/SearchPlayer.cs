using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FaceitRankChecker.Controllers
{
    public class SearchPlayer : Controller
    {
        public async Task<IActionResult> Index()
        {
              return View();

        }
        public async Task<IActionResult> Search(string nickname)
        {

            dynamic deserializedResponse = "";
            using (var client = new HttpClient())
            {
                string auth = "0a486c04-b9c1-406f-b5c2-087d4458a86d";
                client.BaseAddress = new Uri("https://open.faceit.com");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth}");
                //HTTP GET

                var url = $"/data/v4/search/players?nickname={nickname}&offset=0&limit=20";
                var res = await client.GetAsync(url);
                var content = await res.Content.ReadAsStringAsync();

                deserializedResponse = JsonConvert.DeserializeObject(content);
            }

            foreach (var item in deserializedResponse.items)
            {
            ViewData["nickname"] = item.nickname;
            ViewData["level"] = item.games[0].skill_level;
            ViewData["avatar"] = item.avatar;
            }
          

            return View();

        }

    }
}
