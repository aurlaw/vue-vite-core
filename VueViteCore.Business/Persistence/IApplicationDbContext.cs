using Microsoft.EntityFrameworkCore;
using VueViteCore.Business.Entities;

namespace VueViteCore.Business.Persistence;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}