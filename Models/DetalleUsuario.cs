using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desafio_backend.Models
{
    public class DetalleUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetalle { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public string Materia { get; set; }

        // Propiedad de navegación inversa para establecer la relación
        [ForeignKey("IdUsuario")]   
        public virtual Usuario Usuario { get; set; }

    }
}
