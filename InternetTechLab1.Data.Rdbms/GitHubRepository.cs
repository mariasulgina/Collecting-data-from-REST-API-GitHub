using InternetTechLab1.Services;
using InternetTechLab1.Models;
using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace InternetTechLab1.Data.Rdbms;

public class GitHubRepository : IRelationalDatabaseService
{
    private readonly GitHubDbContext _gitHubDbContext = new();

    public void SaveApiGitHubUserInformation(GitHubUser user) 
    {
        _gitHubDbContext.Add(user);
    }

    public void SaveApiGitHubReposInformation(List<GitHubRepo> repos)
    {
        foreach(var repo in repos)
        {
            if (repo.Owner != null)
            {
                _gitHubDbContext.Add(repo.Owner);
                repo.OwnerId = repo.Owner.Id;
                repo.Owner = null;
            }

            _gitHubDbContext.Add(repo);
        }
    }

    public void SaveApiGitHubFollowersInformation(List<GitHubUser> followers)
    {
        foreach (var follower in followers)
        {
            _gitHubDbContext.Add(follower);
        }
    }

    public List<GitHubUser> GetAllUsers() 
    {
        return _gitHubDbContext.Users.ToList();
    }

    public List<GitHubRepo> GetAllRepos()
    {
        return _gitHubDbContext.Repos
            .Include(e => e.Owner)
            .ToList();
    }

    // private void ShowAll<T>(DbSet<T> dbSet) where T : class //можно добавить логику: если filter не пустой
    // {
    //     var data = dbSet.ToList();

    //     if (data.Count == 0) 
    //     {
    //         Clear();
    //         Console.WriteLine($"В базе данных нет {typeof(T).Name}");
    //     } else
    //     {
    //         Console.WriteLine($"\n     Список: {typeof(T).Name}      ");
    //         var properties = typeof(T).GetProperties();

    //         foreach(var elem in data)
    //         {
    //             string dataForElem = "";
    //             foreach(var properti in properties)
    //             {
    //                 if (properti.PropertyType == typeof(string) || properti.PropertyType == typeof(int))
    //                 {
    //                     const string Bold = "\x1b[1m";
    //                     const string Reset = "\x1b[0m";
    //                     dataForElem += $"{Bold}{properti.Name}{Reset} : {properti.GetValue(elem) ?? "null"} ";
    //                 }
    //             }
    //             Console.WriteLine($"      {dataForElem}       \n");
    //             Console.WriteLine(new string('-', 50));
    //         }
    //     }
    // }

    // public void ShowAllUsers() 
    // {
    //     ShowAll(_gitHubDbContext.Users);
    // }

    // public void ShowAllRepos() 
    // {
    //     ShowAll(_gitHubDbContext.Repos);
    // }
}
