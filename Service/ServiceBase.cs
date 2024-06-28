using Microsoft.EntityFrameworkCore;
using Repository;

namespace Service;

public class ServiceBase<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly BookManagementDbContext _context;
    public ServiceBase()
    {
        _context = new BookManagementDbContext();
        _dbSet = _context.Set<T>();
    }


    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }
    
    public bool Add(T entity)
    {
        try
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Remove(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(T entity)
    {
        try
        {
            _context.Update(entity); 
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<T> SearchByKeyword(Func<T, bool> predicate)
    {
        return _dbSet.Where(predicate).ToList();
    }
}