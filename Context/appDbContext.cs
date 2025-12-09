using AgendamentoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoApp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { }

        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
    }
}
