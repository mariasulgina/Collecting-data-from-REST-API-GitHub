using InternetTechLab1.Data;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public class ShowNoSqlDbCommand : ICommand
{
    private INonRelationalDatabaseService _nonRelDb;
    private readonly IVisualizerService _visualizerService;

    public ShowNoSqlDbCommand(INonRelationalDatabaseService nonRelDb, IVisualizerService visualizerService)
    {
        _nonRelDb = nonRelDb;
        _visualizerService = visualizerService;
    }

    public async Task Execute() 
    {
        var webScrapResults = await _nonRelDb.GetAllWebScrapResults();
        _visualizerService.ShowNoSqlDb(webScrapResults);
    }
}
