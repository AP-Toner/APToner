namespace APToner.Models
{
    public class APIResponse<T>
    {
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }
        public T Datos { get; set; }
    }
    public class APIResponseIMG<T>
    {
        public T DATA { get; set; }
        public string MESSAGE { get; set; }
        public bool RESULT { get; set; } 
        public string ERROR { get; set; }
    }
}