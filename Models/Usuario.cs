using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desafio_backend.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("Rol")]
        [Column("IdRol")]
        public int IdRol { get; set; }

        // Propiedad de navegación para el rol
        public virtual Rol Rol { get; set; }

        // Relación con los detalles de usuario (materias)
        public virtual ICollection<DetalleUsuario> DetallesUsuario { get; set; }
    }
}
