using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using TesteAutoGlass.Fornecedores.Application.Commands;
using TesteAutoGlass.Fornecedores.Application.Dtos;
using TesteAutoGlass.Fornecedores.Application.Dtos.Requests;
using TesteAutoGlass.Fornecedores.Application.Dtos.Responses;
using TesteAutoGlass.Fornecedores.Domain.Entities;

namespace TesteAutoGlass.Fornecedores.Application.Mapper
{
    [ExcludeFromCodeCoverage]
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<FornecedorCriacaoDto, CriarFornecedorCommand>();
            CreateMap<FornecedorEdicaoDto, EditarFornecedorCommand>();

            CreateMap<CriarFornecedorCommand, Fornecedor>().
                ForMember(dest => dest.Ativo, map => map.MapFrom(src => true));

            CreateMap<EditarFornecedorCommand, Fornecedor>()
                .ForMember(dest => dest.Ativo, map => map.MapFrom(src => true)); 

            CreateMap<Fornecedor, FornecedorExibicaoDto>();
            CreateMap<Fornecedor, FornecedorListagemDto>();
        }
    }
}
