using AutoMapper;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MarcasAutos.Domain.Marcas;

namespace MarcasAutos.Application.Mapping
{
    public class MarcaAutoMappingProfile : Profile
    {
        public MarcaAutoMappingProfile()
        {
            CreateMap<MarcaAuto, MarcaAutoDto>();
        }
    }
}
