namespace InternetTechLab1.Services;

public class ShowDbCommand : ICommand
{
    private IRelationalDatabaseService _relationalDatabaseService;
    private readonly IVisualizerService _visualizerService;

    public ShowDbCommand(IRelationalDatabaseService relationalDatabaseService, IVisualizerService visualizerService)
    {
        _relationalDatabaseService = relationalDatabaseService;
        _visualizerService = visualizerService;
    }

    public async Task Execute() 
    {
        var users = _relationalDatabaseService.GetAllUsers();
        _visualizer.ShowDataList(users);
    }
}
