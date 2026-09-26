using Dukaan.Application.Interfaces;
using Dukaan.Infrastructure.Data;

namespace Dukaan.Infrastructure.Repositories;

public class Repository<T>: IRepository<T> where T: class
{
    protected readonly AppDbContext _context;
    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}