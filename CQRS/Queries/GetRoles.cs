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
    public class GetRoles
    {
        public class GetRolesQuery : IRequest<List<RolDto>>
        {

        }

        public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RolDto>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public GetRolesQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<RolDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            {
                var roles = await _context.Rols.ToListAsync();
                var rolDto = new List<RolDto>();
                foreach(var rol in roles)
                {
                    var rolDtoItem = new RolDto
                    {
                        Id = rol.id,
                        Descripcion = rol.descripcion
                    };
                    rolDto.Add(rolDtoItem);
                }
                return rolDto;
            }
        }
    }
}
