using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FaceitRankChecker.Controllers
{
    public class FaceitCallbackController : Controller
    {
               
            public async Task<IActionResult> Index(string code)
            {
                string auth = "e9e58299-32c8-425d-9d12-0b61f4955774:ggmW0rmIgTXbZakY1wMU0jcRiquBYpPP9Vu1OzLb";
                string encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.faceit.com");

                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedAuth}");
                    var data = new Dictionary<string, string>
                {
                    {"code", code},
                    {"grant_type", "authorization_code"}
                };

                    var url = "/auth/v1/oauth/token";
                    var res = await client.PostAsync(url, new FormUrlEncodedContent(data));
                    var content = await res.Content.ReadAsStringAsync();


                    dynamic deserializedResponse = JsonConvert.DeserializeObject(content);


                    var stream = deserializedResponse.id_token;
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(stream.ToString());
                    var tokenS = jsonToken as JwtSecurityToken;

                   
                    ViewData["nickname"] = tokenS.Claims.First(c => c.Type == "nickname").Value;
                    ViewData["picture"] = tokenS.Claims.First(c => c.Type == "picture").Value;
                    ViewData["email"] = tokenS.Claims.First(c => c.Type == "email").Value;
                    ViewData["name"] = tokenS.Claims.First(c => c.Type == "given_name").Value;
                    ViewData["familyname"] = tokenS.Claims.First(c => c.Type == "family_name").Value;

                }
                return View();
            }
        }
    }

