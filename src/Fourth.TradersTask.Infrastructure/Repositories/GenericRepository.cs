using Fourth.TradersTask.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fourth.TradersTask.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> Entities;

    public GenericRepository(DbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Entities = context.Set<T>();
    }

    public IQueryable<T> Query() => Entities.AsQueryable();

    public virtual async Task<T?> GetByIdAsync(params object[] keyValues)
    {
        return await Entities.FindAsync(keyValues);
    }

    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Entities.ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        Entities.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        Entities.Remove(entity);
    }
}