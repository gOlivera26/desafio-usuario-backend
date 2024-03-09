
using desafio_backend.Data;
using desafio_backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace api_sistema_de_empleados.CQRS.Commands.DetallesUsuario
{
    public class DeleteDetalleUsuario
    {
        public class DeleteDetalleUsuarioCommand : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class DeleteDetalleUsuarioCommandValidator : AbstractValidator<DeleteDetalleUsuarioCommand>
        {
            private readonly ApplicationContext _context;

            public DeleteDetalleUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("El ID del detalle de usuario es requerido")
                    .MustAsync(ExistDetalleUsuario).WithMessage("El detalle de usuario especificado no existe");
            }

            private async Task<bool> ExistDetalleUsuario(int id, CancellationToken cancellationToken)
            {
                return await _context.DetallesUsuario.AnyAsync(d => d.IdDetalle == id);
            }
        }

        public class DeleteDetalleUsuarioCommandHandler : IRequestHandler<DeleteDetalleUsuarioCommand, Unit>
        {
            private readonly ApplicationContext _context;

            public DeleteDetalleUsuarioCommandHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteDetalleUsuarioCommand request, CancellationToken cancellationToken)
            {
                var detalleUsuario = await _context.DetallesUsuario.FindAsync(request.Id);

                if (detalleUsuario == null)
                {
                    throw new ArgumentException("El detalle de usuario especificado no existe.");
                }

                _context.DetallesUsuario.Remove(detalleUsuario);
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
