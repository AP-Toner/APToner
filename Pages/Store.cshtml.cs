using APToner.Models;
using APToner.Services;
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
        private readonly IMemoryCache _cache;
        private readonly ProductService _productService;

        public StoreModel(ILogger<StoreModel> logger, IMemoryCache cache, ProductService productService)
        {
            _logger = logger;
            _cache = cache;
            _productService = productService;
        }

        public List<Product> Productos { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            if (_cache.TryGetValue("productos", out List<Product> productos))
            {
                Productos = productos;
                return;
            }

            try
            {
                var productosObtenidos = await _productService.GetAllProductsAsync();

                if (productosObtenidos != null && productosObtenidos.Count > 0)
                {
                    _cache.Set("productos", productosObtenidos, TimeSpan.FromMinutes(10));
                    Productos = productosObtenidos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los productos: {ex.Message}");
            }
        }


    }
}
