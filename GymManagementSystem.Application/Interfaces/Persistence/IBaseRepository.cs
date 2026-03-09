using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace GymManagementSystem.Application.Interfaces.Persistence;

public interface IBaseRepository<TEntity, TKey> where TEntity : class
{
    void Add(TEntity entityToAdd);
    Task AddAsync(TEntity entityToAdd);
    void Edit(TEntity entityToEdit);
    Task EditAsync(TEntity entityToEdit);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    void AddRange(IEnumerable<TEntity> entities);
    Task EditRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
    int GetCount(Expression<Func<TEntity, bool>>? filter = null);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null);
    Task<int> GetDistinctCountAsync<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> selector, CancellationToken token = default);

    TEntity? GetById(TKey id);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken token);
    IList<TEntity> GetAll();
    Task<IList<TEntity>> GetAllAsync(CancellationToken token);
    Task<IList<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
    Task<IQueryable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
    TEntity? FindWhere(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> Single(Expression<Func<TEntity, bool>> predicate);

    void Remove(TKey id);
    void Remove(TEntity entityToRemove);
    void Remove(Expression<Func<TEntity, bool>> filter);
    Task RemoveAsync(TKey id);
    Task RemoveAsync(TEntity entityToRemove);
    Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    Task RemoveRangeConditionalAsync(Func<TEntity, bool> predicate);

    void SoftDelete(TKey id);
    void SoftDelete(TEntity entity);
    void SoftDelete(Expression<Func<TEntity, bool>> filter);
    Task SoftDeleteAsync(TKey id);
    Task SoftDeleteAsync(TEntity entity);
    Task SoftDeleteAsync(Expression<Func<TEntity, bool>> filter);

    int Save();
    Task<int> SaveAsync();
}
