using System.Threading.Tasks;
using TesteAutoGlass.Produtos.Application.Dtos.Filters;
using TesteAutoGlass.Produtos.Application.Dtos.Responses;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;

namespace TesteAutoGlass.Produtos.Application.Queries.Abstraction
{
    public interface IProdutoQueries
    {
        Task<PagedResult<ProdutoListagemDto>> ObterTodosPaginadoAsync(PageOptionsProdutoDto pageOptions);
        Task<ProdutoExibicaoDto> ObterPorCodigoAsync(int codigo);
    }
}
