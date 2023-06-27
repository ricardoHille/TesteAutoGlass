using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Context.Abstraction;
using TesteAutoGlass.Infraestruture.Data.Settings;

namespace TesteAutoGlass.Infraestruture.Data.Context
{
    public class ApplicationContext : DbContext, IUnitOfWork
    {
        public ApplicationContext() 
        { }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(ConnectionStrings.DBConnection);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            modelBuilder.HasSequence<int>("ProdutoCodigoSequencial").StartsAt(1).IncrementsBy(1);
            modelBuilder.HasSequence<int>("FornecedorCodigoSequencial").StartsAt(1).IncrementsBy(1);

            base.OnModelCreating(modelBuilder);

        }
        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}
