using APToner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace APToner.Pages
{
    public class StoreModel : PageModel
    {
        private readonly ILogger<StoreModel> _logger;
        private readonly HttpClient _httpClient;

        public StoreModel(ILogger<StoreModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        public List<Product> Productos { get; set; }

        public async Task OnGet()
        {
            var apiUrl = "http://localhost:4000/api/getProductos"; // URL de tu API
            var response = await _httpClient.GetStringAsync(apiUrl);

            // Suponiendo que la respuesta es un JSON que se puede deserializar a una lista de productos
            Productos = JsonConvert.DeserializeObject<List<Product>>(response);
        }
    }
}
