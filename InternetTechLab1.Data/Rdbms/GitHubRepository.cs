using InternetTechLab1.Models;
using Microsoft.EntityFrameworkCore;
using static System.Console;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Data;

public class GitHubRepository : IRelationalDatabaseService
{
    private readonly GitHubDbContext _gitHubDbContext = new();
    private readonly ILoggerService _logger;

    public GitHubRepository(ILoggerService logger)
    {
        _logger = logger;
    }

    public async Task SaveApiGitHubUserInformation(GitHubUser user) 
    {
        await _gitHubDbContext.AddAsync(user);
        await _logger.WriteLogToFile($"[Database] Пользователь {user.Login} успешно добавлен в очередь сохранения");
    }

    public async Task SaveApiGitHubReposInformation(List<GitHubRepo> repos)
    {
        foreach(var repo in repos)
        {
            if (repo.Owner != null)
            {
                await _gitHubDbContext.AddAsync(repo.Owner);
                repo.OwnerId = repo.Owner.Id;
            }

            await _gitHubDbContext.AddAsync(repo);
        }

        await _logger.WriteLogToFile($"[Database] {repos.Count} репозиториев добавлены в очередь сохранения");
    }

    public async Task SaveApiGitHubFollowersInformation(List<GitHubUser> followers)
    {
        foreach (var follower in followers)
        {
            await _gitHubDbContext.AddAsync(follower);
        }

        await _logger.WriteLogToFile($"[Database] {followers.Count} фолловеров добавлены в очередь сохранения");
    }

    public async Task<List<GitHubUser>> GetAllUsers() 
    {
        return await _gitHubDbContext.Users.ToListAsync();
    }

    public async Task<List<GitHubRepo>> GetAllRepos()
    {
        return await _gitHubDbContext.Repos
            .Include(e => e.Owner)
            .ToListAsync();
    }

    public async Task ClearDataBase()
    {
        await _gitHubDbContext.Repos.ExecuteDeleteAsync();
        await _gitHubDbContext.Users.ExecuteDeleteAsync();
        await _logger.WriteLogToFile("[Database] База данных успешно очищена");
    }
}
