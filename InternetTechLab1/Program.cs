using InternetTechLab1.Services;
using InternetTechLab1.Data.NoSql;
using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1;
class Program 
{
    public static async Task Main(string[] args) {
        GitHubApiService gitHubApiService = new();
        GitHubScrapingService gitHubScrapingService = new();
        GitHubRepository gitHubRepository = new();
        ScrapingRepository scrapingRepository = new();
        VisualizerService visualizerService = new();

        MenuService menu = new MenuService(gitHubApiService, gitHubScrapingService, visualizerService, gitHubRepository, scrapingRepository);
        await menu.Run();
    }
}
