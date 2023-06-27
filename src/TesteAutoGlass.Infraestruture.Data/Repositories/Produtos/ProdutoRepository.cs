using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Context;
using TesteAutoGlass.Infraestruture.Data.Repository;
using TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction;
using TesteAutoGlass.Produtos.Domain.Entities;

namespace TesteAutoGlass.Utils.Abstraction.Infra.Data.Repository.Produtos
{
    public class ProdutoRepository : RepositoryGeneric<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public async Task<Produto> ObterPorCodigoAsync(int codigo) =>
            await Query()
                .Include(x => x.Fornecedor)
                .FirstOrDefaultAsync(x => x.Codigo == codigo);
    }
}
