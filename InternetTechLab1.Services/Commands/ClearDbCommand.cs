namespace InternetTechLab1.Services;

public class ClearDbCommand : ICommand
{
    private IDatabaseService _databaseService;

    public ClearDbCommand(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task Execute() 
    {
        await _databaseService.ClearDataBase();
    }
}
