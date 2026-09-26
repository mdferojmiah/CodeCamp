namespace Dukaan.Application.Interfaces;

public interface IRepository<T> where T: class
{
    Task AddAsync(T entity);
    Task SaveChangesAsync();
}