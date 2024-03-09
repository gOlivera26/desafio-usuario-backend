using AutoMapper;
using desafio_backend.Data;
using desafio_backend.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace desafio_backend.CQRS.Queries
{
    public class GetUsuarioFilter
    {
        public class GetUsuarioFilterQuery : IRequest<List<UsuarioDto>>
        {
            public UsuarioFilterDto UsuarioFilter { get; set; }
        }

        public class GetUsuarioFilterQueryHandler : IRequestHandler<GetUsuarioFilterQuery, List<UsuarioDto>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public GetUsuarioFilterQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UsuarioDto>> Handle(GetUsuarioFilterQuery request, CancellationToken cancellationToken)
            {
                var query = _context.Usuarios.AsQueryable();

                if (!string.IsNullOrEmpty(request.UsuarioFilter?.Nombre))
                {
                    query = query.Where(u => u.Nombre.StartsWith(request.UsuarioFilter.Nombre));
                }

                if (!string.IsNullOrEmpty(request.UsuarioFilter?.Email))
                {
                    query = query.Where(u => u.Email.StartsWith(request.UsuarioFilter.Email));
                }

                // Ejecutar la consulta y mapear los resultados a DTOs completos
                var usuarios = await query.Include(u => u.Rol)
                                          .Include(u => u.DetallesUsuario)
                                          .ToListAsync();

                return _mapper.Map<List<UsuarioDto>>(usuarios);
            }

        }
    }
}
