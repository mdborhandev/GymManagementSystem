using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Common;
using GymManagementSystem.Persistence.Context;

namespace GymManagementSystem.Persistence.Repositories;

public class GenericRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>, IGenericRepository<TEntity, TKey>
    where TEntity : BaseModel
{
    public GenericRepository(GymManagementDbContext dbContext) : base(dbContext)
    {
    }
}
