using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace NetSolutions.WebApi.Data.Interceptors;


public class ChangesInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context == null) return result;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return SavingChangesAsync(eventData, result);
    }
}
