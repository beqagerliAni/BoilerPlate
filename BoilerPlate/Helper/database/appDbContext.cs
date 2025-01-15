using BoilerPlate.Helper.database;
using Microsoft.EntityFrameworkCore;
using Modules.User.Entity;
using To_do_List.Helper.Entity;

public class appDbcontext : DbContext
{
    public  required DbSet<UserEntity> User { get; set; }

    public appDbcontext(DbContextOptions options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    baseEntity.DeletedAt = DateTime.UtcNow;
                }
            }
        }
        return base.SaveChanges();
    }

}

