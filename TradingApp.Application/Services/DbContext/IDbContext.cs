using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace TradingApp.Application.Services.Interfaces.Database
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DatabaseFacade Database { get; }

        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;

        Task AddRangeAsync([NotNull] params object[] entities);

        ValueTask<EntityEntry> AddAsync([NotNull] object entity, CancellationToken cancellationToken =
            default);

        EntityEntry Remove([NotNull] object entity);

        void RemoveRange([NotNull] IEnumerable<object> entities);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
