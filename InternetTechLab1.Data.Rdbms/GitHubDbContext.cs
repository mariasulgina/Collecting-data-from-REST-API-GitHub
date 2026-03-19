using Microsoft.EntityFrameworkCore;
using InternetTechLab1.Models;

namespace InternetTechLab1.Data.Rdbms;

public class GitHubDbContext : DbContext
{
    public DbSet<GitHubRepo> Repos { get; set; }
    public DbSet<GitHubUser> Users { get; set; }

    public GitHubDbContext()
    {
        Database.EnsureCreated();
    } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=github_lab.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GitHubRepo>()
            .HasOne(e => e.Owner)
            .WithMany()
            .HasForeignKey(e => e.OwnerId)
            .IsRequired(false);

        modelBuilder.Entity<GitHubUser>()
            .HasMany(e => e.FollowersList)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserFollowers"));
    }

    public void Add<T>(T model) where T : class 
    {
        var id = typeof(T).GetProperty("Id")?.GetValue(model);

        if (id != null)
        {
            var existing = this.Set<T>().Find(id);

            if (existing == null)
            {
                this.Set<T>().Add(model);
            } else
            {
                //если объект уже есть в памяти контекста, говорим EF не пытаться вставлять его или его связи снова
                this.Entry(existing).State = EntityState.Detached; 
                this.Set<T>().Attach(model);
                this.Entry(model).State = EntityState.Modified;
            }

            this.SaveChanges();
        }
    }
}
