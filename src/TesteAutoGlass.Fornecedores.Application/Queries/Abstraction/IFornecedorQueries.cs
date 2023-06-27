using System.Collections.Generic;
using System.Threading.Tasks;
using TesteAutoGlass.Fornecedores.Application.Dtos;
using TesteAutoGlass.Fornecedores.Application.Dtos.Responses;

namespace TesteAutoGlass.Fornecedores.Application.Queries.Abstraction
{
    public interface IFornecedorQueries
    {
        Task<IEnumerable<FornecedorListagemDto>> ObterTodosAsync();
        Task<FornecedorExibicaoDto> ObterPorIdAsync(int id);
    }
}
