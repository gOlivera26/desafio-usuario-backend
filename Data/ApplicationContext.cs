using desafio_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace desafio_backend.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<DetalleUsuario> DetallesUsuario { get; set; }

    }
}
