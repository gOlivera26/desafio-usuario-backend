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
    public class GetDetalleUsuario
    {
        public class GetDetalleUsuarioQuery : IRequest<List<DetalleUsuarioDto>>
        {
            public int IdUsuario { get; set; }
        }

        public class GetDetalleUsuarioQueryHandler : IRequestHandler<GetDetalleUsuarioQuery, List<DetalleUsuarioDto>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public GetDetalleUsuarioQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<DetalleUsuarioDto>> Handle(GetDetalleUsuarioQuery request, CancellationToken cancellationToken)
            {
                var detallesUsuarios = await _context.DetallesUsuario
                    .Where(d => d.IdUsuario == request.IdUsuario)
                    .ToListAsync();

                var detallesUsuarioDto = _mapper.Map<List<DetalleUsuarioDto>>(detallesUsuarios);

               
                foreach (var detalleDto in detallesUsuarioDto)
                {
                    detalleDto.IdDetalle = detalleDto.IdDetalle; // Asignar el ID del detalle al nuevo campo
                }

                return detallesUsuarioDto;
            }

        }
    }
}
