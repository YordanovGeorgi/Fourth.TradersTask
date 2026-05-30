using Fourth.TradersTask.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fourth.TradersTask.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext Context;

    public Repository(DbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable<T> Query() => Context.Set<T>().AsQueryable();

    public virtual async Task<T?> GetByIdAsync(params object[] keyValues)
    {
        return await Context.Set<T>().FindAsync(keyValues);
    }

    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public virtual void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
}