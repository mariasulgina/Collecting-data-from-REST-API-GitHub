using InternetTechLab1.Data.Models;
using InternetTechLab1.Core.Models;
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
        var entity = MapToEntity(user);
        await _gitHubDbContext.AddAsync(entity);
        await _logger.WriteLogToFile($"[Database] Пользователь {user.Login} успешно добавлен в очередь сохранения");
    }

    public async Task SaveApiGitHubReposInformation(List<GitHubRepo> repos)
    {
        foreach(var repo in repos)
        {
            var entity = MapToEntity(repo);
            await _gitHubDbContext.Repos.AddAsync(entity);
        }

        await _logger.WriteLogToFile($"[Database] {repos.Count} репозиториев добавлены в очередь сохранения");
    }

    public async Task SaveApiGitHubFollowersInformation(List<GitHubUser> followers)
    {
        foreach (var follower in followers)
        {
            var entity = MapToEntity(follower);
            _gitHubDbContext.Users.Update(entity);
        }

        await _logger.WriteLogToFile($"[Database] {followers.Count} фолловеров добавлены в очередь сохранения");
    }

    public async Task<List<GitHubUser>> GetAllUsers() 
    {
        var entities = await _gitHubDbContext.Users.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<List<GitHubRepo>> GetAllRepos()
    {
        var entities = await _gitHubDbContext.Repos
            .Include(e => e.Owner)
            .ToListAsync();
        
        return entities.Select(MapToDomain).ToList();
    }

    public async Task ClearDataBase()
    {
        await _gitHubDbContext.Repos.ExecuteDeleteAsync();
        await _gitHubDbContext.Users.ExecuteDeleteAsync();
        await _logger.WriteLogToFile("[Database] База данных успешно очищена");
    }

    public async Task<GitHubUser?> GetUserByLoginAsync(string username)
    {
        var entity = await _gitHubDbContext.Users
            .FirstOrDefaultAsync(u => u.Login != null && u.Login.ToLower() == username.ToLower());
        
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<List<GitHubRepo>?> GetReposByLoginUserAsync(string username)
    {
        var entities = await _gitHubDbContext.Repos
            .Include(r => r.Owner)
            .Where(u => u.Owner != null && u.Owner.Login != null && u.Owner.Login.ToLower() == username.ToLower())
            .ToListAsync();
        
        return entities.Select(MapToDomain).ToList();
    }

    private GitHubUserEntity MapToEntity(GitHubUser user) => new()
    {
        Id = user.Id,
        Login = user.Login,
        AvatarUrl = user.AvatarUrl,
        HtmlUrl = user.HtmlUrl,
        Name = user.Name,
        Company = user.Company,
        Location = user.Location,
        Bio = user.Bio,
        PublicRepos = user.PublicRepos,
        Followers = user.Followers,
        Following = user.Following,
        CreatedAt = user.CreatedAt,
        Email = user.Email
    };

    private GitHubRepoEntity MapToEntity(GitHubRepo repo) => new()
    {
        Id = repo.Id,
        Name = repo.Name,
        FullName = repo.FullName,
        Description = repo.Description,
        HtmlUrl = repo.HtmlUrl,
        IsPrivate = repo.IsPrivate,
        Language = repo.Language,
        StargazersCount = repo.StargazersCount,
        ForksCount = repo.ForksCount,
        CreatedAt = repo.CreatedAt,
        OwnerId = repo.Owner?.Id 
    };

    private GitHubUser MapToDomain(GitHubUserEntity entity) => new()
    {
        Id = entity.Id,
        Login = entity.Login,
        Name = entity.Name,
        AvatarUrl = entity.AvatarUrl,
        Bio = entity.Bio,
        Location = entity.Location,
        Company = entity.Company,
        PublicRepos = entity.PublicRepos,
        Followers = entity.Followers,
        Following = entity.Following,
        CreatedAt = entity.CreatedAt,
        Email = entity.Email
    };

    private GitHubRepo MapToDomain(GitHubRepoEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        FullName = entity.FullName,
        Description = entity.Description,
        Language = entity.Language,
        StargazersCount = entity.StargazersCount,
        ForksCount = entity.ForksCount,
        Owner = entity.Owner != null ? MapToDomain(entity.Owner) : null
    };
}
