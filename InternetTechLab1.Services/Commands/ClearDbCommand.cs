using InternetTechLab1.Data.Rdbms;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class ClearDbCommand : ICommand
{
    public ClearDbCommand(IDatabaseService databaseService, DataType dataType)
    {
        
    }

    public void Execute() 
    {
        
    }
}
