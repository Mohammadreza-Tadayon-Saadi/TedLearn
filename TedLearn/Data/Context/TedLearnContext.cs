using Core.Utilities;
using System.Reflection;
using Data.Entities.BaseEntity;

namespace Data.Context;

public class TedLearnContext : DbContext
{
    public TedLearnContext(DbContextOptions<TedLearnContext> options) : base(options)
    {
        
    }

    #region Override SaveChangeMehod

    public override int SaveChanges()
    {
        _cleanString();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        _cleanString();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _cleanString();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _cleanString();
        return base.SaveChangesAsync(cancellationToken);
    }

    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entitiesAssembly = typeof(IEntity).Assembly;

        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.AddNoActionDeleteBehaviorConvention();
        modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);

        base.OnModelCreating(modelBuilder);
    }

    public void _cleanString()
    {
        var changedEntities = ChangeTracker.Entries()
                                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        foreach (var item in changedEntities)
        {
            if (item.Entity == null)
                continue;

            var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                var propName = property.Name;
                var value = (string)property.GetValue(item.Entity, null);

                if (value.HasValue())
                {
                    var newValue = "";
                    try
                    {
                        newValue = value.Trim().Fa2En().FixPersianChars();
                    }
                    catch
                    {
                        newValue = value.Fa2En().FixPersianChars();
                    }
                    if (newValue == value)
                        continue;

                    property.SetValue(item.Entity, newValue, null);
                }
            }
        }
    }
}
