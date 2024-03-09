using AutoMapper;
using desafio_backend.Dto;
using desafio_backend.Models;
using static desafio_backend.CQRS.Commands.PostUsuario;

namespace desafio_backend.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<DetalleUsuario, DetalleUsuarioDto>().ReverseMap();
            CreateMap<PostUsuarioCommand, Usuario>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.descripcion))
                .ReverseMap(); //Mapear el rol de manera especifica
            
        }
    }
}
