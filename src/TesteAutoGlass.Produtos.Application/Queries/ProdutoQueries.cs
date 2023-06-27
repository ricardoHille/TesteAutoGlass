using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction;
using TesteAutoGlass.Produtos.Application.Dtos.Filters;
using TesteAutoGlass.Produtos.Application.Dtos.Responses;
using TesteAutoGlass.Produtos.Application.Queries.Abstraction;
using TesteAutoGlass.Produtos.Domain.Entities;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;

namespace TesteAutoGlass.Produtos.Application.Queries
{
    public class ProdutoQueries : IProdutoQueries
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutoQueries(
            IProdutoRepository produtoRepository,
            IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;   
        }

        public async Task<ProdutoExibicaoDto> ObterPorCodigoAsync(int codigo)
        {
            var produto = await _produtoRepository.ObterPorCodigoAsync(codigo);

            return _mapper.Map<Produto, ProdutoExibicaoDto>(produto);
        }

        public async Task<PagedResult<ProdutoListagemDto>> ObterTodosPaginadoAsync(PageOptionsProdutoDto pageOptions)
        {
            var query =  _produtoRepository
                            .Query()
                            .Include(x => x.Fornecedor)
                            .Where(x => x.Ativo &&
                                        ((pageOptions.Filtro != null &&
                                        (string.IsNullOrEmpty(pageOptions.Filtro.Nome) || x.Nome.Contains(pageOptions.Filtro.Nome)) &&
                                        (pageOptions.Filtro.FornecedorId == 0 || x.FornecedorId == pageOptions.Filtro.FornecedorId) &&
                                        (pageOptions.Filtro.Codigo == 0 || x.Codigo == pageOptions.Filtro.Codigo)) || pageOptions.Filtro == null))
                            .AsQueryable();

            var produtos = await _produtoRepository.GetPaged(query, pageOptions);

            return _mapper.Map<PagedResult<Produto>, PagedResult<ProdutoListagemDto>>(produtos);
        }
    }
}
