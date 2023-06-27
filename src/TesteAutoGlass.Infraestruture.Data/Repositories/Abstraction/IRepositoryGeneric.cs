using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Context.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Entities.Entities;
using TesteAutoGlass.Utils.Abstractions.Pagination.Options;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;

namespace TesteAutoGlass.Infraestruture.Data.Repository.Abstraction
{
    public interface IRepositoryGeneric<TEntity> 
        where TEntity : EntityBase
    {
        IUnitOfWork UnitOfWork { get; }
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> Query();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task<PagedResult<TEntity>> GetPaged(IQueryable<TEntity> query, PageOptionsDto pageOptionsDto);
    }
}
