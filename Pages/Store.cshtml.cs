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

        public int PaginaActual { get; set; } = 1;
        public int ProductosPorPagina { get; set; } = 21;

        public async Task OnGetAsync(int pagina = 1)
        {
            PaginaActual = pagina;

            if (_cache.TryGetValue("productos", out List<Product> productos))
            {
                Productos = productos.Skip((PaginaActual - 1) * ProductosPorPagina).Take(ProductosPorPagina).ToList();
                return;
            }

            try
            {
                var productosObtenidos = await _productService.GetAllProductsAsync();

                if (productosObtenidos != null && productosObtenidos.Count > 0)
                {
                    _cache.Set("productos", productosObtenidos, TimeSpan.FromMinutes(10));
                    Productos = productosObtenidos.Skip((PaginaActual - 1) * ProductosPorPagina).Take(ProductosPorPagina).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los productos: {ex.Message}");
            }
        }


    }
}
