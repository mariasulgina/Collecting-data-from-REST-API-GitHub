using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;
using InternetTechLab1.Data;
using InternetTechLab1.Services;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public class CommandFactory
{
    private readonly IGitHubService _gitHubService;
    private readonly IScrapingService _scrapingService;
    
    private readonly IVisualizerService _visualizer;
    
    private readonly IRelationalDatabaseService _relDb;
    private readonly INonRelationalDatabaseService _nonRelDb;

    public CommandFactory(
        IGitHubService gitHubService, 
        IScrapingService scrapingService,
        IVisualizerService visualizer,
        IRelationalDatabaseService relDb,
        INonRelationalDatabaseService nonRelDb)
    {
        _gitHubService = gitHubService;
        _scrapingService = scrapingService;
        _visualizer = visualizer;
        _relDb = relDb;
        _nonRelDb = nonRelDb;
    }

    public ICommand? CreateCommand(int mainChoice, int subChoice)
    {
        return (mainChoice, subChoice) switch
        {
            (0, 0) => new GetCurrentUserCommand(_gitHubService, _visualizer),
            (0, 1) => new GetReposCommand(_gitHubService, _visualizer),
            (0, 2) => new GetFollowersCommand(_gitHubService, _visualizer),
            (0, 3) => new ShowDbCommand(_relDb, _visualizer),
            (0, 4) => new ClearDbCommand(_relDb),

            (1, 0) => new WebScrapingCommand(_scrapingService, _visualizer),
            (1, 1) => new ShowNoSqlDbCommand(_nonRelDb, _visualizer),
            (1, 2) => new ClearDbCommand(_nonRelDb),
            _ => null
        };
    }
}