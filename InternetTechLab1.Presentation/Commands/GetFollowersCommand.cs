using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;
using InternetTechLab1.Services;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public class GetFollowersCommand : GetGitHubInformationCommandBase
{
    public GetFollowersCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService)
    : base(gitHubService, visualizerService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        List<GitHubUser>? followers = await Service.GetAndSaveFollowersAsync(username);

        if (followers != null && followers.Count > 0)
        {
            Visualizer.ShowGitHubFollowers(followers, username);
        }
        else
        {
            Console.WriteLine($"У пользователя {username} нет подписчиков или они скрыты");
        }
    }
}
