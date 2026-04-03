using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using InternetTechLab1.Core.Interfaces;
using InternetTechLab1.Core.Settings;

namespace InternetTechLab1.Services;

/// <summary>
/// Сервис для взаимодействия с REST API GitHub.
/// Обеспечивает получение данных о пользователях, репозиториях и других ресурсах.
/// </summary>
public class GitHubApiService : IGitHubApiService
{
    private readonly ILoggerService _logger;
    private readonly GitHubSettings _settings;
    private static readonly HttpClient _httpClient = new HttpClient();

    private static readonly object _lock = new object();
    private static bool _isInitialized = false;

    public GitHubApiService(ILoggerService logger, GitHubSettings settings) 
    {
        _logger = logger;
        _settings = settings;

        // Безопасная инициализация статического клиента один раз
        if (!_isInitialized)
        {
            lock (_lock)
            {
                if (!_isInitialized)
                {
                    InitializeHttpClient();
                    _isInitialized = true;
                }
            }
        }
    }

    /// <summary>
    /// Устанавливает базовые настройки для HttpClient из конфигурационного файла.
    /// </summary>
    private void InitializeHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        
        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _settings.UserAgent);
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("Accept"))
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", _settings.AcceptHeader);
        }

        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
    }

    /// <summary>
    /// Выполняет асинхронный GET-запрос к API GitHub и десериализует ответ в объект заданного типа.
    /// </summary>
    public async Task<T?> GetFromApiGitHubInformationAsync<T>(string endpoint)
    {
        T? gitHubInformation = default;

        try
        {
            await _logger.WriteLogToFileAsync($"[GitHubAPI] Запрос: GET {endpoint}");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_settings.TimeoutSeconds)); 
            gitHubInformation = await _httpClient.GetFromJsonAsync<T>(endpoint, cts.Token);

            return gitHubInformation;
        }
        catch (OperationCanceledException)
        {
            await _logger.WriteLogToFileAsync($"[Error] Время ожидания ({_settings.TimeoutSeconds}с) истекло для {endpoint}");
            throw new Exception("Ошибка: Время ожидания ответа от GitHub истекло");
        }
        catch (HttpRequestException ex)
        {
            await _logger.WriteLogToFileAsync($"[Error] Адресат не найден ({endpoint}): {ex.Message}");
            throw new Exception($"Адресат не найден");
        } 
        catch (Exception ex)
        {
            await _logger.WriteLogToFileAsync($"[Error] Непредвиденная ошибка API ({endpoint}): {ex.Message}");
            throw new Exception($"Ошибка API: {ex.Message}");
        }
    }
}
