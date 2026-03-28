using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;
using InternetTechLab1.Services;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public class GetCurrentUserCommand : GetGitHubInformationCommandBase
{
    public GetCurrentUserCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService)
    : base(gitHubService, visualizerService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        GitHubUser? gitHubUser = await Service.GetAndSaveUserAsync(username);

        if (gitHubUser != null) 
        {
            Visualizer.ShowGitHubUser(gitHubUser);
        } 
        else
        {
            Console.WriteLine($"Пользователь {username} не найден");
        }
    }
}
