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
}
