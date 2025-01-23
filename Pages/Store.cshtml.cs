using APToner.Models;
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
        public List<Product> Productos { get; set; }
        private string apiUrl = "http://localhost:4000/api/getProductos"; // URL de tu API

        private string authToken = "tu_token_de_autorizacion";

        public async Task OnGet()
        {
            // Verifica si los productos están en caché
            if (!_cache.TryGetValue("productos", out List<Product> productos))
            {
                // Crea la solicitud HTTP
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                // Agregar el token al encabezado Authorization
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                var response = await _httpClient.SendAsync(request);

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    productos = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);

                    // Almacenar la respuesta en caché por 24 horas
                    _cache.Set("productos", productos, TimeSpan.FromHours(24));
                }
                else
                {
                    _logger.LogError("Error al obtener los productos. Código de estado: " + response.StatusCode);
                }
            }

            // Asigna los productos obtenidos (ya sea de caché o de la API)
            Productos = productos;
        }
    }
}
