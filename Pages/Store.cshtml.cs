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
        public List<Image> Imagenes { get; set; } = new List<Image>();
        public List<Category> Categorias { get; set; } = new List<Category>();
        public int PaginaActual { get; set; } = 1;
        public int ProductosPorPagina { get; set; } = 21;

        public async Task OnGetAsync(int pagina = 1)
        {
            await obtenerCategorias();
            PaginaActual = pagina;

            if (_cache.TryGetValue("productos", out List<Product> productos))
            {
                if (_cache.TryGetValue("imagenes", out List<Image> imagenes))
                {
                    AsociarImagenesAProductos(productos, imagenes);
                }

                Productos = productos.Skip((PaginaActual - 1) * ProductosPorPagina).Take(ProductosPorPagina).ToList();
                return;
            }

            try
            {
                var productosAPI = await _productService.GetAllProductsAsync();
                var imagenesAPI = await _productService.GetAllImagesAsync();

                if (productosAPI != null && productosAPI.Count > 0)
                {
                    _cache.Set("productos", productosAPI, TimeSpan.FromMinutes(10));
                    _cache.Set("imagenes", imagenesAPI, TimeSpan.FromMinutes(10));

                    AsociarImagenesAProductos(productosAPI, imagenesAPI);

                    Productos = productosAPI.Skip((PaginaActual - 1) * ProductosPorPagina).Take(ProductosPorPagina).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los productos: {ex.Message}");
            }
        }
        private void AsociarImagenesAProductos(List<Product> productos, List<Image> imagenes)
        {
            var imagenesPorSku = imagenes.ToDictionary(
                img => img.SKU,
                img => img.imagenes?.ToList() ?? new List<string>()
            );

            foreach (var item in productos)
            {
                if (imagenesPorSku.TryGetValue(item.Sku, out var urls))
                {
                    item.Imagenes = urls;
                }
                else
                {
                    item.Imagenes = new List<string>();
                }
            }
        }
        public async Task obtenerCategorias()
        {
            try
            {
                var response = await _productService.GetAllCategoriesAsync();

                if (response != null && response.Count > 0)
                {
                    _cache.Set("categorias", response, TimeSpan.FromMinutes(10));
                    Categorias = response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener las categorias: {ex.Message}");
            }
        }
    }
}
