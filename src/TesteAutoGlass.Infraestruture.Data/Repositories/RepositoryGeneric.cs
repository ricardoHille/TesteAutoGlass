using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Context;
using TesteAutoGlass.Infraestruture.Data.Context.Abstraction;
using TesteAutoGlass.Infraestruture.Data.Extensions;
using TesteAutoGlass.Infraestruture.Data.Repository.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Entities.Entities;
using TesteAutoGlass.Utils.Abstractions.Pagination.Options;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;

namespace TesteAutoGlass.Infraestruture.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryGeneric<TEntity> : IRepositoryGeneric<TEntity>
        where TEntity : EntityBase
    {
        private readonly ApplicationContext Context;
        private readonly DbSet<TEntity> _dbSet;
        public IUnitOfWork UnitOfWork => Context;

        protected RepositoryGeneric(ApplicationContext context)
        {
            Context = context;
            _dbSet = context.Set<TEntity>();
        }

        public Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<List<TEntity>> GetAllAsync() => 
            _dbSet.AsNoTracking().ToListAsync();

        public Task<TEntity> GetByIdAsync(int id) =>
            _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task InsertAsync(TEntity entity) =>
            await _dbSet.AddAsync(entity);

        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
        public IQueryable<TEntity> Query() =>
            _dbSet.AsQueryable();

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression) =>
            _dbSet.AsNoTracking().Where(expression);

        public async Task<PagedResult<TEntity>> GetPaged(IQueryable<TEntity> query, PageOptionsDto pageOptionsDto)
        {
            var rowCount = query.CountAsync().Result;
            var result = new PagedResult<TEntity>
            {
                CurrentPage = pageOptionsDto.Page,
                PageSize = pageOptionsDto.Size,
                RowCount = rowCount
            };

            result.PageCount = (int)Math.Ceiling((double)result.RowCount / pageOptionsDto.Size);

            if (pageOptionsDto.Page > result.PageCount && result.PageCount != 0)
            {
                pageOptionsDto.Page = result.PageCount;
            }
            else if (result.PageCount == 0)
            {
                pageOptionsDto.Page = 1;
            }

            var skip = (pageOptionsDto.Page - 1) * pageOptionsDto.Size;

            result.Results = await query
                                   .OrderBy(pageOptionsDto.SortOptions)
                                   .Skip(skip)
                                   .Take(pageOptionsDto.Size)
                                   .ToListAsync();

            return result;
        }
    }
}
