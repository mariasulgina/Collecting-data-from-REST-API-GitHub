namespace InternetTechLab1.Services;

public class GetCurrentUserCommand : ICommand
{
    public GetCurrentUserCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService)
    {

    }

    public void Execute() 
    {
        
    }
}
