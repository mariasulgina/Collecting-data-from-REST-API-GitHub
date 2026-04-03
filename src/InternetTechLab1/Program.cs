using InternetTechLab1.Services;
using InternetTechLab1.Data;
using InternetTechLab1.UI;
using InternetTechLab1.Commands;
using Microsoft.Extensions.Configuration;
using InternetTechLab1.Core.Settings;

namespace InternetTechLab1;

/// <summary>
/// Основной класс приложения, отвечающий за инициализацию конфигурации, 
/// внедрение зависимостей вручную и запуск жизненного цикла программы.
/// </summary>
class Program 
{
    /// <summary>
    /// Главная точка входа в приложение.
    /// </summary>
    public static async Task Main(string[] args) {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var gitHubSettings = configuration.GetSection("GitHubSettings").Get<GitHubSettings>();
        var scrapingSettings = configuration.GetSection("ScrapingSettings").Get<ScrapingSettings>();

        FileLogger loggerService = new FileLogger(configuration);

        //1 слой: данные
        GitHubRepository gitHubDb = new GitHubRepository(loggerService, configuration);
        ScrapingRepository scrapingDb = new ScrapingRepository(loggerService, configuration);

        //2 слой: бизнес-логика
        GitHubApiService api = new GitHubApiService(loggerService, gitHubSettings);
        GitHubService gitHubService = new GitHubService(api, gitHubDb, loggerService);
        ScrapingService scrapingService = new ScrapingService(scrapingDb, loggerService, scrapingSettings);

        //3 слой: презентация и команды
        VisualizerService visualizerService = new();

        CommandFactory factory = new CommandFactory(
            gitHubService, 
            scrapingService, 
            visualizerService, 
            loggerService
        );

        MenuService menu = new MenuService(factory, loggerService);
        await menu.RunAsync();
    }
}
