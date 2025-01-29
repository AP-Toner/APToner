using Newtonsoft.Json;

namespace APToner.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Referencia { get; set; }
        public string Sku { get; set; }
        public string CodigoBarras { get; set; }
        public string CodigoSat { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioOferta { get; set; }
        public decimal PrecioSinOferta { get; set; }
        public bool Oferta { get; set; }
        public string Moneda { get; set; }
        public string MarcaId { get; set; }
        public string MarcaNombre { get; set; }
        public string FamiliaId { get; set; }
        public string FamiliaNombre { get; set; }
        public string SubcategoriaId { get; set; }
        public string SubcategoriaNombre { get; set; }
        public string CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
        public List<string> Imagenes { get; set; } = new List<string>();
    }
    public class Image
    {
        public string SKU { get; set; }
        public string Referencia { get; set; }
        public string[] imagenes { get; set; }
    }

    public class Category
    {
        [JsonProperty("id_categoria")]
        public string Id { get; set; }

        [JsonProperty("nombre_categoria")]
        public string Nombre { get; set; }
    }
    public class Brand
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}
