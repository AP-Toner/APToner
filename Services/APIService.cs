using APToner.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace APToner.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly string authToken = "API_53b415bda0cf1dc46e5c433f897ea9ca9a4c3954";
        private readonly string apiUrl = "http://localhost:4000/api";
        public async Task<APIResponse<T>> GetAsync<T>(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/{endpoint}");
            request.Headers.Add("Authorization", authToken);

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializa el resultado completo, incluyendo "datos" como T
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse<T>>(jsonResponse);

                    return apiResponse;
                }
                else
                {
                    Console.WriteLine($"Error al obtener los datos. Código de estado: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener los datos: {ex.Message}");
                return null;
            }
        }


    }
}
