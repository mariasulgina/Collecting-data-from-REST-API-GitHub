using InternetTechLab1.Services.Models;
using InternetTechLab1.Core.Models;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Services;

/// <summary>
/// Сервис бизнес-логики для работы с данными GitHub.
/// Осуществляет взаимодействие между API-клиентом и базой данных.
/// </summary>
public class GitHubService : IGitHubService
{
    private readonly IGitHubApiService _apiClient;
    private readonly IRelationalDatabaseService _database;
    private readonly ILoggerService _logger;

    public GitHubService(IGitHubApiService apiClient, IRelationalDatabaseService database, ILoggerService logger)
    {
        _apiClient = apiClient;
        _database = database;
        _logger = logger;
    }

    /// <summary>
    /// Получает данные пользователя из API GitHub и сохраняет их в локальную базу данных.
    /// </summary>
    public async Task<GitHubUser?> GetAndSaveUserAsync(string username)
    {
        var user = new GitHubUser();

        await _logger.WriteLogToFileAsync($"[GitHubService] Запрос данных пользователя: {username}");
        
        string endpoint = $"users/{username}";
        var userDto = await _apiClient.GetFromApiGitHubInformationAsync<GitHubUserDto>(endpoint);

        if (userDto != null)
        {
            user = MapToDomainUser(userDto);

            await _logger.WriteLogToFileAsync($"[GitHubService] Данные {username} получены. Сохранение в БД...");
            await _database.SaveApiGitHubUserInformationAsync(user);
        }
        else 
        {
            await _logger.WriteLogToFileAsync($"[GitHubService] Пользователь {username} не найден в API GitHub");
        }

        return user;
    }

    /// <summary>
    /// Запрашивает список подписчиков пользователя из API и сохраняет их в базу данных.
    /// </summary>
    public async Task<List<GitHubUser>?> GetAndSaveFollowersAsync(string username)
    {
        var followers = new List<GitHubUser>();

        await _logger.WriteLogToFileAsync($"[GitHubService] Запрос фолловеров для: {username}");
        
        string endpoint = $"users/{username}/followers";
        var followersDto = await _apiClient.GetFromApiGitHubInformationAsync<List<GitHubUserDto>>(endpoint);

        if (followersDto != null && followersDto.Count > 0)
        {
            followers = followersDto.Select(user => MapToDomainUser(user)).ToList();

            await _logger.WriteLogToFileAsync($"[GitHubService] Получено {followers.Count} фолловеров для {username}. Сохранение...");
            await _database.SaveApiGitHubFollowersInformationAsync(followers); 
        }
        else 
        {
            await _logger.WriteLogToFileAsync($"[GitHubService] У пользователя {username} фолловеры не найдены или список пуст.");
        }

        return followers;
    }

    /// <summary>
    /// Запрашивает список публичных репозиториев пользователя из API и сохраняет их в базу данных.
    /// </summary>
    public async Task<List<GitHubRepo>?> GetAndSaveReposAsync(string username)
    {
        var repos = new List<GitHubRepo>();

        await _logger.WriteLogToFileAsync($"[GitHubService] Запрос репозиториев для: {username}");
        
        string endpoint = $"users/{username}/repos";
        var reposDto = await _apiClient.GetFromApiGitHubInformationAsync<List<GitHubRepoDto>>(endpoint);

        if (reposDto != null && reposDto.Count > 0)
        {
            repos = reposDto.Select(repo => MapToDomainRepo(repo)).ToList();

            await _logger.WriteLogToFileAsync($"[GitHubService] Получено {repos.Count} репозиториев для {username}. Сохранение...");
            await _database.SaveApiGitHubReposInformationAsync(repos);
        }
        else 
        {
            await _logger.WriteLogToFileAsync($"[GitHubService] У пользователя {username} репозитории не найдены.");
        }

        return repos;
    }

    /// <summary>
    /// Ищет информацию о пользователе в локальной базе данных.
    /// </summary>
    public async Task<GitHubUser?> GetUserByLoginAsync(string username)
    {
        GitHubUser? user = await _database.GetUserByLoginAsync(username);
        return user;
    }

    /// <summary>
    /// Получает список репозиториев пользователя, ранее сохраненных в базе данных.
    /// </summary>
    public async Task<List<GitHubRepo>?> GetReposByLoginUserAsync(string username)
    {
        List<GitHubRepo>? repos = await _database.GetReposByLoginUserAsync(username);
        return repos;
    }

    /// <summary>
    /// Полностью очищает все таблицы, связанные с GitHub, в локальной базе данных.
    /// </summary>
    public async Task ClearAllDataAsync() 
    {
        await _logger.WriteLogToFileAsync("[GitHubService] Очистка SQL базы...");
        await _database.ClearDataBaseAsync();
    }

    /// <summary>
    /// Извлекает всех пользователей, когда-либо сохраненных в локальную базу данных.
    /// </summary>
    public async Task<IEnumerable<GitHubUser>> GetSavedUsersAsync()
    {
        await _logger.WriteLogToFileAsync("[GitHubService] Чтение всех пользователей из SQL...");
        return await _database.GetAllUsersAsync(); 
    }

    /// <summary>
    /// Извлекает все репозитории, сохраненные в локальную базу данных.
    /// </summary>
    public async Task<IEnumerable<GitHubRepo>> GetSavedReposAsync()
    {
        await _logger.WriteLogToFileAsync("[GitHubService] Чтение всех репозиториев из SQL...");
        return await _database.GetAllReposAsync();
    }

    /// <summary>
    /// Преобразует объект передачи данных (DTO) в доменную модель пользователя.
    /// </summary>
    private GitHubUser MapToDomainUser(GitHubUserDto dto) => new()
    {
        Id = dto.Id,
        Login = dto.Login,
        Name = dto.Name,
        AvatarUrl = dto.AvatarUrl,
        Bio = dto.Bio,
        Location = dto.Location,
        Company = dto.Company,
        PublicRepos = dto.PublicRepos,
        Followers = dto.Followers,
        Following = dto.Following,
        CreatedAt = dto.CreatedAt,
        Email = dto.Email
    };

    /// <summary>
    /// Преобразует объект передачи данных (DTO) в доменную модель репозитория.
    /// </summary>
    private GitHubRepo MapToDomainRepo(GitHubRepoDto dto) 
    {
        return new GitHubRepo 
        {
            Id = dto.Id,
            Name = dto.Name,
            FullName = dto.FullName,
            Description = dto.Description,
            Language = dto.Language,
            StargazersCount = dto.StargazersCount,
            ForksCount = dto.ForksCount,
            Owner = dto.Owner != null ? MapToDomainUser(dto.Owner) : null 
        };
    }
}
