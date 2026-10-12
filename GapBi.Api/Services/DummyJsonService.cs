
using System.Net.Http.Json;
using System.Text.Json;

namespace GapBi.Api.Services
{
    public class DummyJsonService
    {
        private readonly HttpClient httpClient;

        public DummyJsonService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<JsonElement> BuscarDadosAsync(string endpoint)
        {
            var resposta = await httpClient.GetAsync(
                $"https://dummyjson.com/{endpoint}?limit=0"
            );

            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content
                .ReadFromJsonAsync<JsonElement>();

            return dados;
        }
    }
}
