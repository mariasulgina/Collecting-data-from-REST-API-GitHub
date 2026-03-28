using InternetTechLab1.Services;
using InternetTechLab1.Data;
using InternetTechLab1.UI;
using InternetTechLab1.Commands;

namespace InternetTechLab1;

class Program 
{
    public static async Task Main(string[] args) {
        //1 слой
        GitHubApiService api = new();
        GitHubRepository gitHubDb = new();
        ScrapingRepository scrapingDb = new();

        //2 слой
        GitHubService gitHubService = new GitHubService(api, gitHubDb);
        ScrapingService scrapingService = new ScrapingService(scrapingDb);

        //3 слой
        VisualizerService visualizerService = new();

        CommandFactory factory = new CommandFactory(
            gitHubService, 
            scrapingService, 
            visualizerService, 
            gitHubDb, 
            scrapingDb
        );

        MenuService menu = new MenuService(factory);
        await menu.Run();
    }
}
