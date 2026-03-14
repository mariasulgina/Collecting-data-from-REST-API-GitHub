namespace InternetTechLab1.Services;

public class GetFollowersCommand : ICommand
{
    public GetFollowersCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService)
    {

    }

    //массив из GitHubUser
    public void Execute() 
    {
        
    }
}
