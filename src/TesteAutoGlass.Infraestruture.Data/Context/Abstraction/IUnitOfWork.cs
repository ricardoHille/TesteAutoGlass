using System.Threading.Tasks;

namespace TesteAutoGlass.Infraestruture.Data.Context.Abstraction
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
