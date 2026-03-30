using InternetTechLab1.Models;

using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Services;

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

    public async Task<GitHubUser?> GetAndSaveUserAsync(string username)
    {
        try 
        {
            await _logger.WriteLogToFile($"[GitHubService] Запрос данных пользователя: {username}");
            
            string endpoint = $"users/{username}";
            var user = await _apiClient.GetFromApiGitHubInformationAsync<GitHubUser>(endpoint);

            if (user != null)
            {
                await _logger.WriteLogToFile($"[GitHubService] Данные {username} получены. Сохранение в БД...");
                await _database.SaveApiGitHubUserInformation(user);
            }
            else 
            {
                await _logger.WriteLogToFile($"[GitHubService] Пользователь {username} не найден в API GitHub.");
            }

            return user;
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Ошибка при работе с пользователем {username}: {ex.Message}");
            throw;
        }
    }

    public async Task<List<GitHubUser>?> GetAndSaveFollowersAsync(string username)
    {
        try 
        {
            await _logger.WriteLogToFile($"[GitHubService] Запрос фолловеров для: {username}");
            
            string endpoint = $"users/{username}/followers";
            var followers = await _apiClient.GetFromApiGitHubInformationAsync<List<GitHubUser>>(endpoint);

            if (followers != null && followers.Count > 0)
            {
                await _logger.WriteLogToFile($"[GitHubService] Получено {followers.Count} фолловеров для {username}. Сохранение...");
                await _database.SaveApiGitHubFollowersInformation(followers); 
            }
            else 
            {
                await _logger.WriteLogToFile($"[GitHubService] У пользователя {username} фолловеры не найдены или список пуст.");
            }

            return followers;
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Ошибка при получении фолловеров {username}: {ex.Message}");
            throw;
        }
    }

    public async Task<List<GitHubRepo>?> GetAndSaveReposAsync(string username)
    {
        try 
        {
            await _logger.WriteLogToFile($"[GitHubService] Запрос репозиториев для: {username}");
            
            string endpoint = $"users/{username}/repos";
            var repos = await _apiClient.GetFromApiGitHubInformationAsync<List<GitHubRepo>>(endpoint);

            if (repos != null && repos.Count > 0)
            {
                await _logger.WriteLogToFile($"[GitHubService] Получено {repos.Count} репозиториев для {username}. Сохранение...");
                await _database.SaveApiGitHubReposInformation(repos);
            }
            else 
            {
                await _logger.WriteLogToFile($"[GitHubService] У пользователя {username} репозитории не найдены.");
            }

            return repos;
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Ошибка при получении репозиториев {username}: {ex.Message}");
            throw;
        }
    }
}
