namespace AgroSolidario.Models
{
    public class Alimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string Doador { get; set; }
        public string Destino { get; set; }
        public DateTime Validade { get; set; }
        public string? Foto { get; set; }
        public string Status { get; set; } = "Pendente";
    }
}