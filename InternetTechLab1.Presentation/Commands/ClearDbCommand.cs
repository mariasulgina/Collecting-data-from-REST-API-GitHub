using InternetTechLab1.Data;

namespace InternetTechLab1.Commands;

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
