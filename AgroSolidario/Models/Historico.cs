namespace AgroSolidario.Models
{
    public class Historico
    {
        public int Id { get; set; }
        public string NomeAlimento { get; set; }
        public int Quantidade { get; set; }
        public string Doador { get; set; }
        public string Destino { get; set; }
        public DateTime DataDoacao { get; set; } = DateTime.Now;
    }
}