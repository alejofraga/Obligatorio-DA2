using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic;

namespace SmartHome.DataLayer;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public bool Exist(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Any(predicate);
    }

    public void Add(TEntity entity)
    {
        _entities.Add(entity);
        context.SaveChanges();
    }

    public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null
            ? _entities.ToList()
            : _entities.Where(predicate).ToList();
    }

    public virtual TEntity? GetOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = _entities.FirstOrDefault(predicate);
        return entity;
    }

    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);

        context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
        context.SaveChanges();
    }
}
