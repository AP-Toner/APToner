namespace APToner.Models
{
    public class APIResponse<T>
    {
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }
        public T Datos { get; set; }
    }
}