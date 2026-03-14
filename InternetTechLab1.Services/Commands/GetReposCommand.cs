namespace InternetTechLab1.Services;

public class GetReposCommand : ICommand
{
    public GetReposCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService)
    {

    }

    public void Execute() 
    {
        
    }
}
