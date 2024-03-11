using AutoMapper;
using desafio_backend.Data;
using desafio_backend.Dto;
using desafio_backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace desafio_backend.CQRS.Commands
{
    public class PostRol
    {
        public class PostRolCommand : IRequest<RolDto>
        {
            public string Nombre { get; set; }
        }
        public class PostRolCommandValidator : AbstractValidator<PostRolCommand>
        {
            private readonly ApplicationContext _context;

            public PostRolCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
                RuleFor(x => x).MustAsync(RolExiste).WithMessage("El rol ya existe");
                
            }

            private async Task<bool> RolExiste(PostRolCommand command, CancellationToken token)
            {
                bool existe = await _context.Rols.AnyAsync(x => x.descripcion == command.Nombre);
                return !existe;
            }

        }
        public class PostRolCommandHandler : IRequestHandler<PostRolCommand, RolDto>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PostRolCommand> _validator;

            public PostRolCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostRolCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<RolDto> Handle(PostRolCommand request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                var rol = new Rol
                {
                    descripcion = request.Nombre
                };
                _context.Rols.Add(rol);
                await _context.SaveChangesAsync();
                var rolDto = _mapper.Map<RolDto>(rol);
                return rolDto;
            }
        }
    }
}
