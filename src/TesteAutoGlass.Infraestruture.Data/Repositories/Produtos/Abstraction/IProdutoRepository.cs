using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Repository.Abstraction;
using TesteAutoGlass.Produtos.Domain.Entities;

namespace TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction
{
    public interface IProdutoRepository : IRepositoryGeneric<Produto>
    {
        Task<Produto> ObterPorCodigoAsync(int codigo);
    }
}
