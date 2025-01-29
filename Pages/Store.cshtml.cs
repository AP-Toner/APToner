using APToner.Models;
using APToner.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<Subcategory> Subcategorias { get; set; } = new List<Subcategory>();
        public List<Brand> Marcas { get; set; } = new List<Brand>();
        public int PaginaActual { get; set; } = 1;
        public int ProductosPorPagina { get; set; } = 21;

        public async Task OnGetAsync(int pagina = 1)
        {
            PaginaActual = pagina;

            // Ejecutar ambas funciones en paralelo para mejorar el rendimiento
            await Task.WhenAll(obtenerFiltros(), obtenerProductosConImagenes());
        }

        private async Task obtenerFiltros()
        {

            try
            {
                if (!_cache.TryGetValue("marcas", out List<Brand> marcasAPI))
                {
                    marcasAPI = await _productService.GetAllBranchesAsync();

                    if (marcasAPI != null && marcasAPI.Count > 0)
                    {
                        _cache.Set("marcas", marcasAPI, TimeSpan.FromMinutes(10));
                    }
                }
                Marcas = marcasAPI ?? new List<Brand>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener las marcas: {ex.Message}");
            }
            try
            {
                if (!_cache.TryGetValue("categorias", out List<Category> categoriasAPI))
                {
                    categoriasAPI = await _productService.GetAllCategoriesAsync();

                    if (categoriasAPI != null && categoriasAPI.Count > 0)
                    {
                        _cache.Set("categorias", categoriasAPI, TimeSpan.FromMinutes(10));
                    }
                }
                Categorias = categoriasAPI ?? new List<Category>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener las categorías: {ex.Message}");
            }

            try
            {
                if (!_cache.TryGetValue("subcategorias", out List<Subcategory> subcategoriasAPI))
                {
                    subcategoriasAPI = await _productService.GetAllSubcategoriesAsync();

                    if (subcategoriasAPI != null && subcategoriasAPI.Count > 0)
                    {
                        _cache.Set("subcategorias", subcategoriasAPI, TimeSpan.FromMinutes(10));
                    }
                }
                Subcategorias = subcategoriasAPI ?? new List<Subcategory>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener las marcas: {ex.Message}");
            }
        }

        private async Task obtenerProductosConImagenes()
        {
            try
            {
                if (!_cache.TryGetValue("productos", out List<Product> productosAPI))
                {
                    productosAPI = await _productService.GetAllProductsAsync();
                    _cache.Set("productos", productosAPI, TimeSpan.FromMinutes(10));
                }

                if (!_cache.TryGetValue("imagenes", out List<Image> imagenesAPI))
                {
                    imagenesAPI = await _productService.GetAllImagesAsync();
                    _cache.Set("imagenes", imagenesAPI, TimeSpan.FromMinutes(10));
                }

                // Asociar imágenes a productos si ambos datos están disponibles
                if (productosAPI != null && imagenesAPI != null)
                {
                    AsociarImagenesAProductos(productosAPI, imagenesAPI);
                }

                Productos = productosAPI?.Skip((PaginaActual - 1) * ProductosPorPagina)
                                        .Take(ProductosPorPagina)
                                        .ToList() ?? new List<Product>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los productos e imágenes: {ex.Message}");
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
    }
}
