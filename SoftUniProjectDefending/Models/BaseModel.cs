using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace FaceitRankChecker.Models
{
    public class BaseModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseModel(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;
        public async Task OnGet()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                "https://open.faceit.com/data/v4/players?nickname=Marulqta")
            {
                Headers =
            {
                { HeaderNames.Accept, "application/json" },
                { HeaderNames.Authorization, "Bearer 0a486c04-b9c1-406f-b5c2-087d4458a86d" }
            }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
            }
        }
    }
}
