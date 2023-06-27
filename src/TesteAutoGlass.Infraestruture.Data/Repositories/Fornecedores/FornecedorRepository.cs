using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Infraestruture.Data.Context;
using TesteAutoGlass.Infraestruture.Data.Repository;
using TesteAutoGlass.Infraestruture.Data.Repository.Fornecedores.Abstraction;

namespace TesteAutoGlass.Utils.Abstraction.Infra.Data.Repository.Fornecedores
{
    public class FornecedorRepository : RepositoryGeneric<Fornecedor>, IFornecedorRepository
    {
        
        public FornecedorRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
