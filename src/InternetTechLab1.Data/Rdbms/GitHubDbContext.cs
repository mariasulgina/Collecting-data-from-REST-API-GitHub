using Microsoft.EntityFrameworkCore;
using InternetTechLab1.Data.Models;
using Microsoft.Extensions.Configuration;

namespace InternetTechLab1.Data;

public class GitHubDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<GitHubRepoEntity> Repos { get; set; }
    public DbSet<GitHubUserEntity> Users { get; set; }

    public GitHubDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GitHubRepoEntity>()
            .HasOne(e => e.Owner)
            .WithMany()
            .HasForeignKey(e => e.OwnerId)
            .IsRequired(false);

        modelBuilder.Entity<GitHubUserEntity>()
            .HasMany(e => e.FollowersList)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserFollowers"));
    }

    public async Task AddAsync<T>(T model) where T : class 
    {
        var id = typeof(T).GetProperty("Id")?.GetValue(model);

        if (id != null)
        {
            var existing = await this.Set<T>().FindAsync(id);

            if (existing == null)
            {
                await this.Set<T>().AddAsync(model);
            } 
            else
            {
                //если объект уже есть в памяти контекста, говорим EF не пытаться вставлять его или его связи снова
                this.Entry(existing).State = EntityState.Detached; 
                this.Set<T>().Attach(model);
                this.Entry(model).State = EntityState.Modified;
            }

            await this.SaveChangesAsync();
        }
    }
}
