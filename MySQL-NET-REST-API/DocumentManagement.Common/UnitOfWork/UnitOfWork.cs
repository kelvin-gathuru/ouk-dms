using System;
using System.Threading.Tasks;
using DocumentManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Common.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UnitOfWork<TContext>> _logger;
    public UnitOfWork(
        TContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UnitOfWork<TContext>> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    public int Save()
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
       {
           try
           {
               using (var transaction = _context.Database.BeginTransaction())
               {
                   try
                   {
                       SetModifiedInformation();
                       var val = _context.SaveChanges();
                       transaction.Commit();
                       return val;
                   }
                   catch (Exception e)
                   {
                       transaction.Rollback();
                       _logger.LogError(e, "Database save error");
                       return -1;
                   }
               }
           }
           catch (Exception e)
           {
               _logger.LogError(e, "Database save error");
               return -1;
           }
       });
        return -1;
    }

    public bool Migration()
    {
        _context.Database.Migrate();
        return true;

    }
    public async Task<int> SaveAsync()
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        SetModifiedInformation();
                        var val = await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return val;
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, "Database save error");
                        return -1;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database save error");
                return -1;
            }
        });
    }
    public TContext Context => _context;
    public void Dispose()
    {
        _context.Dispose();
    }

    private void SetModifiedInformation()
    {
        foreach (var entry in Context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.Items["Id"])))
                {
                    entry.Entity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext.Items["Id"].ToString());
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity.IsDeleted)
                {
                    if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.Items["Id"])))
                    {
                        entry.Entity.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext.Items["Id"].ToString());
                    }
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                }
                else
                {
                    if (_httpContextAccessor.HttpContext != null && !string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.Items["Id"])))
                    {
                        entry.Entity.ModifiedBy = Guid.Parse(_httpContextAccessor.HttpContext.Items["Id"].ToString());
                    }
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
