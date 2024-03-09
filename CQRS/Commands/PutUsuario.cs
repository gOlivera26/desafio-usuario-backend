using desafio_backend.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace desafio_backend.CQRS.Commands
{
    public class PutUsuario
    {
        public class PutUsuarioCommand : IRequest<Unit>
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
            }

            private async Task<bool> ExistUsuario(int id, CancellationToken cancellationToken)
            {
                return await _context.Usuarios.AnyAsync(u => u.Id == id);
            }
        }

        public class PutUsuarioCommandHandler : IRequestHandler<PutUsuarioCommand, Unit>
        {
            private readonly ApplicationContext _context;

            public PutUsuarioCommandHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(PutUsuarioCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _context.Usuarios.FindAsync(request.Id);

                if (usuario == null)
                {
                    throw new ArgumentException("El usuario especificado no existe.");
                }

                usuario.Nombre = request.Nombre;
                usuario.Email = request.Email;
         
                var rol = await _context.Rols.FindAsync(request.IdRol);
                if (rol == null)
                {
                    throw new ArgumentException("El rol especificado no existe.");
                }

                usuario.Rol = rol;

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }

    }
}
