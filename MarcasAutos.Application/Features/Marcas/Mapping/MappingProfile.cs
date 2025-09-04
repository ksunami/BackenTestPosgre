using AutoMapper;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MarcasAutos.Domain.Marcas;

namespace MarcasAutos.Application.Features.Marcas.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MarcaAuto, MarcaAutoDto>();
        }
    }
}
