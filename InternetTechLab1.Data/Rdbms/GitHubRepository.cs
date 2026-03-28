using InternetTechLab1.Models;
using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace InternetTechLab1.Data;

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

    public async Task ClearDataBase()
    {
        await _gitHubDbContext.Repos.ExecuteDeleteAsync();
        await _gitHubDbContext.Users.ExecuteDeleteAsync();
    }
}
