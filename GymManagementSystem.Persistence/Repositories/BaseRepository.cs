using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Common;
using GymManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GymManagementSystem.Persistence.Repositories;

public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : BaseModel
{
    protected readonly GymManagementDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    protected int CommandTimeout { get; set; }

    public BaseRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();

        CommandTimeout = 300;
    }
    public virtual void Add(TEntity entityToAdd)
    {
        _dbSet.Add(entityToAdd);
    }

    public virtual async Task AddAsync(TEntity entityToAdd)
    {
        await _dbSet.AddAsync(entityToAdd);
    }

    public virtual void Edit(TEntity entityToEdit)
    {
        _dbSet.Attach(entityToEdit);
        _dbContext.Entry(entityToEdit).State = EntityState.Modified;
    }

    public virtual async Task<IQueryable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return await Task.FromResult(_dbSet.Where(predicate).AsQueryable());

    }

    public virtual async Task EditAsync(TEntity entityToEdit)
    {
        await Task.Run(() =>
        {
            _dbSet.Attach(entityToEdit);
            _dbContext.Entry(entityToEdit).State = EntityState.Modified;
        });
    }
    public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool isTrakingOff = false)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            var result = orderBy(query);

            if (isTrakingOff)
                return result.AsNoTracking().ToList();
            else
                return result.ToList();
        }
        else
        {
            if (isTrakingOff)
                return query.AsNoTracking().ToList();
            else
                return query.ToList();
        }

    }

    public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        return query.AsTracking().ToList();
    }

    public virtual (IList<TEntity> data, int total, int totalDisplay) Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 1, int pageSize = 10, bool isTrakingOff = false)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);
        var total = query.Count();
        var totalDisplay = query.Count();

        if (filter != null)
        {
            query = query.Where(filter);
            totalDisplay = query.Count();
        }

        if (include != null)
            query = include(query);

        if (orderBy != null)
        {
            var result = orderBy(query).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrakingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
        else
        {
            var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrakingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
    }

    public virtual IList<TEntity> GetAll()
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);
        return query.ToList();
    }

    public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken token)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);
        return await query.ToListAsync(token);
    }

    public virtual async Task<IList<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
    {
        IQueryable<TEntity> query = _dbSet
            .Where(x => !x.IsDelete)
            .Where(predicate);
        return await query.ToListAsync(token);
    }


    public virtual async Task<IList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool isTrackingOff = false)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            var result = orderBy(query);

            if (isTrackingOff)
                return await result.AsNoTracking().ToListAsync();
            else
                return await result.ToListAsync();
        }
        else
        {
            if (isTrackingOff)
                return await query.AsNoTracking().ToListAsync();
            else
                return await query.ToListAsync();
        }
    }


    public virtual async Task<IList<TEntity>> GetAsync(
        CancellationToken token,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync(token);
    }


    public virtual async Task<(IList<TEntity> data, int total, int totalDisplay)> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 1,
        int pageSize = 10,
        bool isTrackingOff = false)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);

        var total = await query.CountAsync();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var totalDisplay = await query.CountAsync();

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        if (isTrackingOff)
        {
            query = query.AsNoTracking();
        }

        var data = await query.ToListAsync();

        return (data, total, totalDisplay);
    }


    public virtual TEntity? GetById(TKey id)
    {
        return _dbSet.FirstOrDefault(x =>
            !x.IsDelete &&
            EF.Property<TKey>(x, "Id")!.Equals(id));
    }
    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken token)
    {
        return await _dbSet.FirstOrDefaultAsync(x =>
           !x.IsDelete &&
            EF.Property<TKey>(x, "Id")!.Equals(id), token);
    }
    public virtual int GetCount(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);

        return filter != null
            ? query.Count(filter)
            : query.Count();
    }
    public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = _dbSet.Where(x => !x.IsDelete);

        return filter != null
            ? await query.CountAsync(filter)
            : await query.CountAsync();
    }

    public virtual void Remove(TKey id)
    {
        var entityToDelete = _dbSet.Find(id);
        if (entityToDelete != null)
            Remove(entityToDelete);
    }

    public virtual void Remove(TEntity entityToRemove)
    {
        if (_dbContext.Entry(entityToRemove).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToRemove);
        }

        _dbSet.Remove(entityToRemove);
    }

    public virtual void Remove(Expression<Func<TEntity, bool>> filter)
    {
        _dbSet.RemoveRange(_dbSet.Where(filter));
    }

    public virtual async Task RemoveAsync(TKey id)
    {
        var entityToRemove = await _dbSet.FindAsync(id);
        if (entityToRemove != null)
            await RemoveAsync(entityToRemove);
    }

    public virtual async Task RemoveAsync(TEntity entityToRemove)
    {
        await Task.Run(() =>
        {
            if (_dbContext.Entry(entityToRemove).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToRemove);
            }

            _dbSet.Remove(entityToRemove);
        });
    }

    public virtual async Task RemoveAsync(Expression<Func<TEntity, bool>> filter)
    {
        await Task.Run(() =>
        {
            _dbSet.RemoveRange(_dbSet.Where(filter));
        });
    }

    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken token,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbSet;

        query = query.Where(x => !x.IsDelete);

        if (disableTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = orderBy(query);

        return await query.Select(selector).FirstOrDefaultAsync(token);
    }

    public virtual async Task<IEnumerable<TResult>> GetAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default) where TResult : class
    {
        IQueryable<TEntity> query = _dbSet;

        query = query.Where(x => !x.IsDelete);

        if (disableTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = orderBy(query);

        return await query.Select(selector).ToListAsync(cancellationToken);
    }

    public TEntity? FindWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(x => !x.IsDelete).Where(predicate).AsNoTracking().FirstOrDefault();
    }

    public virtual int Save()
    {
        return _dbContext.SaveChanges();
    }

    public virtual async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
    public async Task<TEntity?> Single(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }


    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        _dbSet.RemoveRange(entities);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        _dbSet.RemoveRange(entities);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        _dbSet.AddRange(entities);
    }

    public async Task RemoveRangeConditionalAsync(Func<TEntity, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var entities = _dbSet.Where(predicate).ToList();
        if (entities == null || !entities.Any())
            throw new InvalidOperationException("No entities found to remove.");

        _dbSet.RemoveRange(entities);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
    {
        return await _dbSet.AnyAsync(predicate, token);
    }


    public virtual void SoftDelete(TKey id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }

    public virtual void SoftDelete(TEntity entity)
    {
        entity.IsDelete = true;
        entity.DateDeleted = DateTime.UtcNow;
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual void SoftDelete(Expression<Func<TEntity, bool>> filter)
    {
        var entities = _dbSet.Where(filter).ToList();
        foreach (var entity in entities)
        {
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }

    public virtual async Task SoftDeleteAsync(TKey id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }

    public virtual async Task SoftDeleteAsync(TEntity entity)
    {
        await Task.Run(() =>
        {
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
        });
    }

    public virtual async Task SoftDeleteAsync(Expression<Func<TEntity, bool>> filter)
    {
        var query = _dbSet.Where(filter);

        await query.ExecuteUpdateAsync(setters => setters
            .SetProperty(e => e.IsDelete, true)
            .SetProperty(e => e.DateDeleted, _ => DateTime.UtcNow));
    }

    public async Task<int> GetDistinctCountAsync<TProperty>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TProperty>> selector,
        CancellationToken token = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query
            .Select(selector)
            .Distinct()
            .CountAsync(token);
    }

    public async Task EditRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return;

        foreach (var entity in entities)
        {
            _dbSet.Update(entity);
        }
    }
}
