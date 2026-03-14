using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1.Services;

public class GetFollowersCommand : ICommand
{
    public GetFollowersCommand(IGitHubApiService gitHubApiService, IDatabaseService databaseService)
    {

    }

    //массив из GitHubUser
    public void Execute() 
    {
        
    }
}
