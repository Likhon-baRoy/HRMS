using API.Exceptions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class DbSetExtensions
{
    public static async Task<T> GetByIdOrThrowAsync<T>(this DbSet<T> dbSet, int id) where T : BaseTrackableEntity
    {
        return await dbSet.FindAsync(id)
            ?? throw new NotFoundException(typeof(T).Name, id);
    }
}
