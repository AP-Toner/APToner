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


    }
}
