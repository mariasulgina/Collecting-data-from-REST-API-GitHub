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
            .HasForeignKey("OwnerId")
            .IsRequired(false);
    }

    public void Add<T>(T model) where T : class
    {
        var idValue = typeof(T).GetProperty("Id").GetValue(model);
        if (idValue != null && this.Set<T>().Find(idValue) == null) 
        {
            this.Set<T>().Add(model);
            this.SaveChanges();
        } else
        {
            Console.WriteLine($"[DB] Запись {typeof(T).Name} с ID {idValue} уже существует");
        }
    }

    public void AddRanges<T>(IEnumerable<T> models) where T : class
    {
        this.Set<T>().AddRange(models);
        this.SaveChanges();
    }
}
