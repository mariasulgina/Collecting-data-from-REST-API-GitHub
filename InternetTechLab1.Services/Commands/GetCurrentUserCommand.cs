using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1.Services;

public class GetCurrentUserCommand : ICommand
{
    public GetCurrentUserCommand(IGitHubApiService gitHubApiService, IDatabaseService databaseService)
    {

    }

    public void Execute() 
    {
        
    }
}
