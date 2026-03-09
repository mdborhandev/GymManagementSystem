using GymManagementSystem.Domain.Common;

namespace GymManagementSystem.Application.Interfaces.Persistence;

public interface IGenericRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
    where TEntity : BaseModel
{
}
