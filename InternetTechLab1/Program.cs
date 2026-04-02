using InternetTechLab1.Services;
using InternetTechLab1.Data;
using InternetTechLab1.UI;
using InternetTechLab1.Commands;
using Microsoft.Extensions.Configuration;

namespace InternetTechLab1;

class Program 
{
    public static async Task Main(string[] args) {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        FileLogger loggerService = new FileLogger();

        //1 слой
        GitHubApiService api = new GitHubApiService(loggerService);
        GitHubRepository gitHubDb = new GitHubRepository(loggerService, configuration);
        ScrapingRepository scrapingDb = new ScrapingRepository(loggerService, configuration);

        //2 слой
        GitHubService gitHubService = new GitHubService(api, gitHubDb, loggerService);
        ScrapingService scrapingService = new ScrapingService(scrapingDb, loggerService);

        //3 слой
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
