using APToner.Models;
using Newtonsoft.Json;
using APToner.Services;

namespace APToner.Services
{
    public class ProductService
    {
        private readonly APIService _apiService;
        public ProductService(APIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<List<Product>>("productos");

                if (response != null && response.Resultado && response.Datos != null)
                {
                    return response.Datos;
                }
                else
                {
                    Console.WriteLine($"Error al obtener los productos: {response?.Mensaje}");
                    return new List<Product>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener los productos: {ex.Message}");
                return new List<Product>();
            }
        }
        public async Task<List<Image>> GetAllImagesAsync()
        {
            try
            {
                var response = await _apiService.GetIMGAsync<List<Image>>();

                if (response != null && response.RESULT && response.DATA != null)
                {
                    return response.DATA;
                }
                else
                {
                    Console.WriteLine($"Error al obtener los productos: {response?.MESSAGE}");
                    return new List<Image>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener los productos: {ex.Message}");
                return new List<Image>();
            }
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<List<Category>>("categorias");
                Console.WriteLine($"json: {response}");

                if (response != null && response.Resultado && response.Datos != null)
                {
                    return response.Datos;
                }
                else
                {
                    Console.WriteLine($"Error al obtener las categorias: {response?.Mensaje}");
                    return new List<Category>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener las categorias: {ex.Message}");
                return new List<Category>();
            }
        }
        public async Task<List<Subcategory>> GetAllSubcategoriesAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<List<Subcategory>>("subcategorias");
                Console.WriteLine($"json: {response}");

                if (response != null && response.Resultado && response.Datos != null)
                {
                    return response.Datos;
                }
                else
                {
                    Console.WriteLine($"Error al obtener las categorias: {response?.Mensaje}");
                    return new List<Subcategory>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener las categorias: {ex.Message}");
                return new List<Subcategory>();
            }
        }
        public async Task<List<Brand>> GetAllBranchesAsync()
        {
            try
            {
                var response = await _apiService.GetAsync<List<Brand>>("marcas");
                Console.WriteLine($"json: {response}");

                if (response != null && response.Resultado && response.Datos != null)
                {
                    return response.Datos;
                }
                else
                {
                    Console.WriteLine($"Error al obtener las marcas: {response?.Mensaje}");
                    return new List<Brand>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener las marcas: {ex.Message}");
                return new List<Brand>();
            }
        }
    }
}
