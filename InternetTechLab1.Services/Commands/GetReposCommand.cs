using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1.Services;

public class GetReposCommand : ICommand
{
    public GetReposCommand(IGitHubApiService gitHubApiService, IDatabaseService databaseService)
    {

    }

    public void Execute() 
    {
        
    }
}
