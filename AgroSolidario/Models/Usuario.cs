namespace AgroSolidario.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; }

        // Campos extras para Beneficiário
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? TipoInstituicao { get; set; }
    }
}