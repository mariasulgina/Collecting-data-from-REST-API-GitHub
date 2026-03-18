namespace InternetTechLab1.Services;

public class ShowDbCommand : ICommand
{
    private IRelationalDatabaseService _relationalDatabaseService;

    public ShowDbCommand(IRelationalDatabaseService relationalDatabaseService)
    {
        _relationalDatabaseService = relationalDatabaseService;
    }

    public async Task Execute() 
    {
        _relationalDatabaseService.ShowAllUsers();
    }
}
