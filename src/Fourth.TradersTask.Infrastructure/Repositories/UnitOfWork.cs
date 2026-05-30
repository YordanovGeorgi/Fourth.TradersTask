using Fourth.TradersTask.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Fourth.TradersTask.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        var repo = (IRepository<T>)_repositories.GetOrAdd(type, _ =>
        {
            return new Repository<T>(_context);
        });

        return repo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}