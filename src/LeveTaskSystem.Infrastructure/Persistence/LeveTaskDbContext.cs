using LeveTaskSystem.Domain.Entities;
using LeveTaskSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeveTaskSystem.Infrastructure.Persistence;

public class LeveTaskDbContext(DbContextOptions<LeveTaskDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserTask> Tasks => Set<UserTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.LandlinePhone).HasMaxLength(20);
            entity.Property(x => x.MobilePhone).HasMaxLength(20);
            entity.Property(x => x.Address).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PhotoPath).HasMaxLength(300);
            entity.Property(x => x.PasswordHash).HasMaxLength(300).IsRequired();
            entity.Property(x => x.Role).HasConversion<int>().IsRequired();
            entity.HasOne(x => x.Manager).WithMany().HasForeignKey(x => x.ManagerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Message).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Status).HasConversion<int>().IsRequired();
            entity.HasOne(x => x.AssignedToUser).WithMany(x => x.AssignedTasks).HasForeignKey(x => x.AssignedToUserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.CreatedByManager).WithMany(x => x.CreatedTasks).HasForeignKey(x => x.CreatedByManagerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.Parse("8ad687a0-c30e-4198-bfdb-4f23f57be320"),
            FullName = "Usuario Gestor Inicial",
            BirthDate = new DateTime(1990, 1, 1),
            LandlinePhone = "0000000000",
            MobilePhone = "00000000000",
            Email = "ti@leveinvestimentos.com.br",
            Address = "Endereco padrao",
            PhotoPath = string.Empty,
            PasswordHash = "100000.zjL3MhmhY2X8FcCYjNse5Q==.9R/7dgljXliM6h7D4QJI43nQ4Gx55SyJ8ygffM0j9Q0=",
            Role = UserRole.Manager
        });
    }
}
