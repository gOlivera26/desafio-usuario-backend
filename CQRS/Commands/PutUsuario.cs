using AutoMapper;
using desafio_backend.Data;
using desafio_backend.Dto;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace desafio_backend.CQRS.Commands
{
    public class PutUsuario
    {
        public class PutUsuarioCommand : IRequest<UsuarioDto>
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public int IdRol { get; set; }
        }

        public class PutUsuarioCommandValidator : AbstractValidator<PutUsuarioCommand>
        {
            private readonly ApplicationContext _context;

            public PutUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("El ID del usuario es requerido")
                    .MustAsync(ExistUsuario).WithMessage("El usuario especificado no existe");

                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("El nombre del usuario es requerido")
                    .MaximumLength(100).WithMessage("El nombre del usuario no puede tener más de 100 caracteres");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("El email del usuario es requerido")
                    .EmailAddress().WithMessage("El formato del email no es válido");

                RuleFor(x => x.IdRol)
                    .NotEmpty().WithMessage("El ID del rol es requerido")
                    .MustAsync(ExistRol).WithMessage("El rol especificado no existe");
            }

            private async Task<bool> ExistUsuario(int id, CancellationToken cancellationToken)
            {
                return await _context.Usuarios.AnyAsync(u => u.Id == id);
            }

            private async Task<bool> ExistRol(int idRol, CancellationToken cancellationToken)
            {
                return await _context.Rols.AnyAsync(r => r.id == idRol);
            }
        }

        public class PutUsuarioCommandHandler : IRequestHandler<PutUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PutUsuarioCommand> _validator;

            public PutUsuarioCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PutUsuarioCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<UsuarioDto> Handle(PutUsuarioCommand request, CancellationToken cancellationToken)
            {
                await _validator.ValidateAndThrowAsync(request); // Aplicar validaciones

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (usuario == null)
                {
                    throw new ArgumentException("El usuario especificado no existe.");
                }

                usuario.Nombre = request.Nombre;
                usuario.Email = request.Email;
                usuario.IdRol = request.IdRol;

                await _context.SaveChangesAsync();

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                return usuarioDto;
            }
        }
    }
}
