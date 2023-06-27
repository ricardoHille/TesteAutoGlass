using AutoMapper;
using System.Linq;
using TesteAutoGlass.Produtos.Application.Commands;
using TesteAutoGlass.Produtos.Application.Dtos.Requests;
using TesteAutoGlass.Produtos.Application.Dtos.Responses;
using TesteAutoGlass.Produtos.Domain.Entities;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;

namespace TesteAutoGlass.Produtos.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<ProdutoCriacaoDto, CriarProdutoCommand>();
            CreateMap<ProdutoEdicaoDto, EditarProdutoCommand>();

            CreateMap<CriarProdutoCommand, Produto>().
                ForMember(dest => dest.Ativo, map => map.MapFrom(src => true));

            CreateMap<EditarProdutoCommand, Produto>().
                ForMember(dest => dest.Ativo, map => map.MapFrom(src => true)); ;

            CreateMap<Produto, ProdutoExibicaoDto>()
                .ForMember(dest => dest.Fabricacao, map => map.MapFrom(src => src.Fabricacao.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Validade, map => map.MapFrom(src => src.Validade.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.FornecedorId, map => map.MapFrom(src => src.FornecedorId))
                .ForMember(dest => dest.Fornecedor, map => map.MapFrom(src => $"{src.Fornecedor.Codigo} - {src.Fornecedor.Nome}"));

            CreateMap<Produto, ProdutoListagemDto>()
                .ForMember(dest => dest.Fabricacao, map => map.MapFrom(src => src.Fabricacao.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Validade, map => map.MapFrom(src => src.Validade.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Fornecedor, map => map.MapFrom(src => $"{src.Fornecedor.Codigo} - {src.Fornecedor.Nome}"));

            CreateMap<PagedResult<Produto>, PagedResult<ProdutoListagemDto>>()
                .ForMember(dest => dest.CurrentPage, map => map.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.PageSize, map => map.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.PageCount, map => map.MapFrom(src => src.PageCount))
                .ForMember(dest => dest.RowCount, map => map.MapFrom(src => src.RowCount))
                .ForMember(dest => dest.Results, map => map.MapFrom(src => src.Results.Select(x => new ProdutoListagemDto
                {
                    Codigo = x.Codigo,
                    Fabricacao = x.Fabricacao.ToString("dd/MM/yyyy"),
                    Fornecedor = x.Fornecedor.Nome,
                    Id = x.Id,
                    Nome = x.Nome,
                    Validade = x.Validade.ToString("dd/MM/yyyy")
                }).ToList()));
        }
    }
}
