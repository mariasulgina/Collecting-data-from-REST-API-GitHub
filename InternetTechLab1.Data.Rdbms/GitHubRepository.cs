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
        _gitHubDbContext.AddRanges(repos);
    }

    public void SaveApiGitHubFollowersInformation(List<GitHubUser> followers)
    {
        _gitHubDbContext.AddRanges(followers);
    }

    private void ShowAll<T>(DbSet<T> dbSet) where T : class //можно добавить логику: если filter не пустой
    {
        var data = dbSet.ToList();

        if (data.Count == 0) 
        {
            Clear();
            Console.WriteLine($"В базе данных нет {typeof(T).Name}");
        } else
        {
            Console.WriteLine($"\n     Список: {typeof(T).Name}      ");
            var properties = typeof(T).GetProperties();

            foreach(var elem in data)
            {
                string dataForElem = "";
                foreach(var properti in properties)
                {
                    if (properti.PropertyType == typeof(string) || properti.PropertyType == typeof(int))
                    {
                        const string Bold = "\x1b[1m";
                        const string Reset = "\x1b[0m";
                        dataForElem += $"{Bold}{properti.Name}{Reset} : {properti.GetValue(elem) ?? "null"} ";
                    }
                }
                Console.WriteLine($"      {dataForElem}       \n");
                Console.WriteLine(new string('-', 50));
            }
        }
    }

    public void ShowAllUsers() 
    {
        ShowAll(_gitHubDbContext.Users);
    }

    public void ShowAllRepos() 
    {
        ShowAll(_gitHubDbContext.Repos);
    }
}
