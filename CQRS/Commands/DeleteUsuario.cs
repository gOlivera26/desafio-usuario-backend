using desafio_backend.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace desafio_backend.CQRS.Commands
{
    public class DeleteUsuario
    {
        public class DeleteUsuarioCommand : IRequest<string>
        {
            public int Id { get; set; }
        }

        public class DeleteUsuarioCommandValidator : AbstractValidator<DeleteUsuarioCommand>
        {
            private readonly ApplicationContext _context;

            public DeleteUsuarioCommandValidator(ApplicationContext context)
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

        public class DeleteUsuarioCommandHandler : IRequestHandler<DeleteUsuarioCommand, string>
        {
            private readonly ApplicationContext _context;

            public DeleteUsuarioCommandHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _context.Usuarios.FindAsync(request.Id);

                if (usuario == null)
                {
                    throw new ArgumentException("El usuario especificado no existe.");
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return "Usuario eliminado con éxito.";
            }
        }
    }
}
