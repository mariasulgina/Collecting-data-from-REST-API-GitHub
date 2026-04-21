using InternetTechLab1.Data.Models;
using InternetTechLab1.Core.Models;
using Microsoft.EntityFrameworkCore;
using static System.Console;
using InternetTechLab1.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using InternetTechLab1.Data.Mappers;

namespace InternetTechLab1.Data;

/// <summary>
/// Репозиторий для управления данными GitHub в реляционной базе данных SQLite.
/// </summary>
public class GitHubRepository : IRelationalDatabaseService
{
    private readonly IConfiguration _configuration;
    private readonly GitHubDbContext _gitHubDbContext;
    private readonly ILoggerService _logger;

    public GitHubRepository(ILoggerService logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _gitHubDbContext = new GitHubDbContext(_configuration);
    }

    /// <summary>
    /// Сохраняет или обновляет информацию о пользователе GitHub в базе данных.
    /// Перед сохранением очищается трекер состояний EF для предотвращения конфликтов.
    /// </summary>
    public async Task SaveApiGitHubUserInformationAsync(GitHubUser user) 
    {
        _gitHubDbContext.ChangeTracker.Clear();

        var existing = await _gitHubDbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        var entity = user.MapToEntity();

        if (existing == null) {
            await _gitHubDbContext.Users.AddAsync(entity);
        } else {
            _gitHubDbContext.Users.Update(entity);
        }

        await _gitHubDbContext.SaveChangesAsync();
        await _logger.WriteLogToFileAsync($"[Database] Пользователь {user.Login} успешно добавлен в очередь сохранения");
    }

    /// <summary>
    /// Сохраняет список репозиториев пользователя. 
    /// Для каждого репозитория проверяется его наличие в БД по ID.
    /// </summary>
    public async Task SaveApiGitHubReposInformationAsync(List<GitHubRepo> repos)
    {
        _gitHubDbContext.ChangeTracker.Clear();

        foreach(var repo in repos)
        {
            var entity = repo.MapToEntity();
            var existing = await _gitHubDbContext.Repos.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == repo.Id);

            if (existing == null) {
                await _gitHubDbContext.Repos.AddAsync(entity);
            } else {
                _gitHubDbContext.Repos.Update(entity);
            }
        }

        await _gitHubDbContext.SaveChangesAsync();
        await _logger.WriteLogToFileAsync($"[Database] {repos.Count} репозиториев добавлены в очередь сохранения");
    }

    /// <summary>
    /// Сохраняет список подписчиков как сущностей пользователей.
    /// </summary>
    public async Task SaveApiGitHubFollowersInformationAsync(List<GitHubUser> followers)
    {
        _gitHubDbContext.ChangeTracker.Clear();

        foreach (var follower in followers)
        {
            var entity = follower.MapToEntity();
            var existing = await _gitHubDbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == follower.Id);

            if (existing == null) {
                await _gitHubDbContext.Users.AddAsync(entity);
            } else {
                _gitHubDbContext.Users.Update(entity);
            }
        }

        await _gitHubDbContext.SaveChangesAsync();
        await _logger.WriteLogToFileAsync($"[Database] {followers.Count} фолловеров добавлены в очередь сохранения");
    }

    /// <summary>
    /// Извлекает всех пользователей из базы данных.
    /// </summary>
    public async Task<List<GitHubUser>> GetAllUsersAsync() 
    {
        var entities = await _gitHubDbContext.Users.AsNoTracking().ToListAsync();
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    /// <summary>
    /// Извлекает все репозитории вместе с информацией об их владельцах.
    /// </summary>
    public async Task<List<GitHubRepo>> GetAllReposAsync()
    {
        var entities = await _gitHubDbContext.Repos
            .AsNoTracking()
            .Include(e => e.Owner)
            .ToListAsync();
        
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    /// <summary>
    /// Полностью очищает таблицы репозиториев и пользователей в БД.
    /// </summary>
    public async Task ClearDataBaseAsync()
    {
        await _gitHubDbContext.Repos.ExecuteDeleteAsync();
        await _gitHubDbContext.Users.ExecuteDeleteAsync();

        await _logger.WriteLogToFileAsync("[Database] База данных успешно очищена");
    }

    /// <summary>
    /// Поиск пользователя в базе данных по его логину без учета регистра.
    /// </summary>
    public async Task<GitHubUser?> GetUserByLoginAsync(string username)
    {
        var entity = await _gitHubDbContext.Users
            .FirstOrDefaultAsync(u => u.Login != null && u.Login.ToLower() == username.ToLower());
        
        return entity == null ? null : entity.MapToDomain();
    }

    /// <summary>
    /// Получение всех репозиториев, принадлежащих конкретному пользователю по его логину.
    /// </summary>
    public async Task<List<GitHubRepo>?> GetReposByLoginUserAsync(string username)
    {
        var entities = await _gitHubDbContext.Repos
            .Include(r => r.Owner)
            .Where(u => u.Owner != null && u.Owner.Login != null && u.Owner.Login.ToLower() == username.ToLower())
            .ToListAsync();
        
        return entities.Select(e => e.MapToDomain()).ToList();
    }
}
