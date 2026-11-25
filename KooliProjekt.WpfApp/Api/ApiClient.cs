using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApp.Api
{
    // Suhtleb veebirakenduse Web API-ga (api/BeerTypes)
    [ExcludeFromCodeCoverage]
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
        }

        public async Task<Result<List<BeerType>>> List()
        {
            var result = new Result<List<BeerType>>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<BeerType>>("BeerTypes");
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<Result> Save(BeerType item)
        {
            var result = new Result();

            try
            {
                HttpResponseMessage response;
                if (item.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("BeerTypes", item);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync("BeerTypes/" + item.Id, item);
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<Result> Delete(int id)
        {
            var result = new Result();

            try
            {
                var response = await _httpClient.DeleteAsync("BeerTypes/" + id);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
    }
}
