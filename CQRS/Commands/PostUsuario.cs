using AutoMapper;
using desafio_backend.Data;
using desafio_backend.Dto;
using desafio_backend.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace desafio_backend.CQRS.Commands
{
    public class PostUsuario
    {
        public class PostUsuarioCommand  : IRequest<UsuarioDto>
        {
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public int IdRol { get; set; }
            
        }
        public class PostUsuarioCommandValidator : AbstractValidator<PostUsuarioCommand>
        {
            private readonly ApplicationContext _context;
            public PostUsuarioCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
                RuleFor(x => x.Email).NotEmpty().WithMessage("El email es requerido");
                RuleFor(x => x.Email).EmailAddress().WithMessage("El email no es valido");
                RuleFor(x => x.Email).MustAsync(async (email, cancellation) =>
                {
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
                    return usuario == null;
                }).WithMessage("El email ya esta en uso");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El password es requerido");
                RuleFor(x => x.IdRol).NotEmpty().WithMessage("El rol es requerido");
                RuleFor(x => x.IdRol).MustAsync(async (idRol, cancellation) =>
                {
                    var rol = await _context.Rols.FirstOrDefaultAsync(x => x.id == idRol);
                    return rol != null;
                }).WithMessage("El rol no existe");
                RuleFor(x => x).MustAsync(UsuarioExiste).WithMessage("El usuario ya existe");
            }

            private async Task<bool> UsuarioExiste(PostUsuarioCommand command, CancellationToken token)
            {
                bool existe = await _context.Usuarios.AnyAsync(x => x.Nombre == command.Nombre && x.Email == command.Email);
                return !existe;
            }

        }
        public class PostUsuarioCommandHandler : IRequestHandler<PostUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PostUsuarioCommand> _validator;

            public PostUsuarioCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostUsuarioCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<UsuarioDto> Handle(PostUsuarioCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var usuarioResult = await _validator.ValidateAsync(request);
                    if (!usuarioResult.IsValid)
                    {
                        throw new ValidationException(usuarioResult.Errors);
                    }
           
                    var usuario = new Usuario
                    {
                        Nombre = request.Nombre,
                        Email = request.Email,
                        Password = request.Password,
                        IdRol = request.IdRol
                    };
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    // Mapear el nuevo usuario a UsuarioDto
                    var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                    return usuarioDto;
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y relanzarla para que sea capturada por el middleware de excepciones
                    throw new Exception("Error al crear el usuario.", ex);
                }
            }

        }
    }
}
