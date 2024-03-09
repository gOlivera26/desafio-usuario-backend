using AutoMapper;
using desafio_backend.Data;
using desafio_backend.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace desafio_backend.CQRS.Queries
{
    public class GetUsuario
    {
        public class GetUsuarioQuery : IRequest<List<UsuarioDto>>
        {

        }
        public class GetUsuarioQueryHandler : IRequestHandler<GetUsuarioQuery, List<UsuarioDto>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public GetUsuarioQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UsuarioDto>> Handle(GetUsuarioQuery request, CancellationToken cancellationToken)
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                var usuariosDto = new List<UsuarioDto>();

                foreach (var usuario in usuarios)
                {
                    var rol = await _context.Rols.FirstOrDefaultAsync(x => x.id == usuario.IdRol);
                    var detallesUsuario = await _context.DetallesUsuario.Where(x => x.IdUsuario == usuario.Id).ToListAsync();

                    var usuarioDto = new UsuarioDto
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre,
                        Email = usuario.Email,
                        Rol = rol?.descripcion, 
                        Materias = detallesUsuario.Select(d => d.Materia).ToList() 
                    };

                    usuariosDto.Add(usuarioDto);
                }

                return usuariosDto;
            }


        }
    }
}
