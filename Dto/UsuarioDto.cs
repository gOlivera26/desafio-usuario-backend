namespace desafio_backend.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public List<string> Materias { get; set; }
    }
}
