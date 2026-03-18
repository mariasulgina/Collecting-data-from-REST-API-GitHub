using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class GetFollowersCommand : GetGitHubInformationCommandBase
{
    public GetFollowersCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService, 
    IVisualizerService visualizerService)
    : base(gitHubApiService, relationalDatabaseService, visualizerService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        string endpoint = $"users/{username}/followers";

        List<GitHubUser>? followers = await ApiService.GetFromApiGitHubInformationAsync<List<GitHubUser>>(endpoint);

        if (followers != null && followers.Count > 0)
        {
            DbService.SaveApiGitHubFollowersInformation(followers);
            Visualizer.ShowGitHubFollowers(followers, username);
        }
        else
        {
            Console.WriteLine($"У пользователя {username} нет подписчиков или они скрыты");
        }
    }
}
