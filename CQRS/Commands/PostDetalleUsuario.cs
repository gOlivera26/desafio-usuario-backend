using desafio_backend.Data;
using desafio_backend.Dto;
using desafio_backend.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace desafio_backend.CQRS.Commands
{
    public class PostDetalleUsuario
    {
        public class PostDetalleUsuarioCommand : IRequest<DetalleUsuarioDto>
        {
            public int IdUsuario { get; set; }
            public string Materia { get; set; }
        }

        public class PostDetalleUsuarioCommandHandler : IRequestHandler<PostDetalleUsuarioCommand, DetalleUsuarioDto>
        {
            private readonly ApplicationContext _context;

            public PostDetalleUsuarioCommandHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<DetalleUsuarioDto> Handle(PostDetalleUsuarioCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var usuarioExistente = await _context.Usuarios.FindAsync(request.IdUsuario);
                    if (usuarioExistente == null)
                    {
                        throw new ArgumentException("El usuario especificado no existe.");
                    }

                    var detalleUsuario = new DetalleUsuario
                    {
                        IdUsuario = request.IdUsuario,
                        Materia = request.Materia
                    };

                    _context.DetallesUsuario.Add(detalleUsuario);
                    await _context.SaveChangesAsync();

                    return new DetalleUsuarioDto
                    {
                        IdUsuario = detalleUsuario.IdUsuario,
                        Materia = detalleUsuario.Materia
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el detalle de usuario.", ex);
                }
            }
        }
    }
}
