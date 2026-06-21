using Microsoft.EntityFrameworkCore;
using AgroSolidario.Models;

namespace AgroSolidario.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Alimento> Alimentos { get; set; }
        public DbSet<Historico> Historicos { get; set; }
    }
}