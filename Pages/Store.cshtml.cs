using APToner.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace APToner.Pages
{
    public class StoreModel : PageModel
    {
        private readonly ILogger<StoreModel> _logger;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public StoreModel(ILogger<StoreModel> logger, HttpClient httpClient, IMemoryCache cache)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cache = cache;
        }

        public List<Product> Productos { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            var authToken = "API_53b415bda0cf1dc46e5c433f897ea9ca9a4c3954";
            var apiUrl = "http://localhost:4000/api/getProductos";

            // Verifica si los productos están en caché
            if (!_cache.TryGetValue("productos", out List<Product> productos))
            {
                try
                {
                    // Configura el cliente HTTP

                    // Envía la solicitud GET
                    var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                    request.Headers.Add("Authorization", authToken);

                    var response = await _httpClient.SendAsync(request);

                    // Verificar si la respuesta es exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserializa el objeto completo y accede a la propiedad "resultado"
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse>(jsonResponse);
                        productos = apiResponse.datos;

                        // Almacenar en caché
                        _cache.Set("productos", productos, TimeSpan.FromHours(24));
                    }
                    else
                    {
                        _logger.LogError($"Error al obtener los productos. Código de estado: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrió un error al realizar la solicitud: {ex.Message}");
                }
            }

            // Asigna los productos obtenidos (ya sea de caché o de la API)
            Productos = productos;
        }
    }
}
