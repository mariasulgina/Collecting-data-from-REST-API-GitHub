using System.Threading.Tasks;
using InternetTechLab1.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class CommandFactory
{
    private readonly IVisualizerService _visualizer;

    private readonly IGitHubService _gitHubService;
    private readonly IScrapingService _scrapingService;
    private readonly ILoggerService _loggerService;
    
    private readonly IRelationalDatabaseService _relDb;
    private readonly INonRelationalDatabaseService _nonRelDb;

    public CommandFactory(
        IGitHubService gitHubService, 
        IScrapingService scrapingService,
        IVisualizerService visualizer,
        IRelationalDatabaseService relDb,
        INonRelationalDatabaseService nonRelDb,
        ILoggerService loggerService)
    {
        _gitHubService = gitHubService;
        _scrapingService = scrapingService;
        _visualizer = visualizer;
        _relDb = relDb;
        _nonRelDb = nonRelDb;
        _loggerService = loggerService;
    }

    public ICommand? CreateCommand(int mainChoice, int subChoice)
    {
        return (mainChoice, subChoice) switch
        {
            (0, 0) => new GetCurrentUserCommand(_gitHubService, _visualizer, _loggerService),
            (0, 1) => new GetReposCommand(_gitHubService, _visualizer, _loggerService),
            (0, 2) => new GetFollowersCommand(_gitHubService, _visualizer, _loggerService),
            (0, 3) => new ShowDbCommand(_relDb, _visualizer, _loggerService),
            (0, 4) => new ClearDbCommand(_relDb, _loggerService),

            (1, 0) => new WebScrapingCommand(_scrapingService, _visualizer, _loggerService),
            (1, 1) => new ShowNoSqlDbCommand(_nonRelDb, _visualizer, _loggerService),
            (1, 2) => new ClearDbCommand(_nonRelDb, _loggerService),
            _ => null
        };
    }
}
