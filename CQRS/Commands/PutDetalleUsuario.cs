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
    public class PutDetalleUsuario
    {
        public class PutDetalleUsuarioCommand : IRequest<Unit>
        {
            public int Id { get; set; }
            public int IdUsuario { get; set; }
            public string Materia { get; set; }
        }

        public class PutDetalleUsuarioCommandValidator : AbstractValidator<PutDetalleUsuarioCommand>
        {
            private readonly ApplicationContext _context;

            public PutDetalleUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("El ID del detalle de usuario es requerido")
                    .MustAsync(ExistDetalleUsuario).WithMessage("El detalle de usuario especificado no existe");

                RuleFor(x => x.IdUsuario)
                    .MustAsync(ExistUsuario).WithMessage("El usuario especificado no existe");
            }

            private async Task<bool> ExistDetalleUsuario(int id, CancellationToken cancellationToken)
            {
                return await _context.DetallesUsuario.AnyAsync(d => d.IdDetalle == id);
            }

            private async Task<bool> ExistUsuario(int id, CancellationToken cancellationToken)
            {
                return await _context.Usuarios.AnyAsync(u => u.Id == id);
            }
        }

        public class PutDetalleUsuarioCommandHandler : IRequestHandler<PutDetalleUsuarioCommand, Unit>
        {
            private readonly ApplicationContext _context;

            public PutDetalleUsuarioCommandHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(PutDetalleUsuarioCommand request, CancellationToken cancellationToken)
            {
                var detalleUsuario = await _context.DetallesUsuario.FindAsync(request.Id);

                if (detalleUsuario == null)
                {
                    throw new ArgumentException("El detalle de usuario especificado no existe.");
                }
             
                detalleUsuario.Materia = request.Materia;

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
