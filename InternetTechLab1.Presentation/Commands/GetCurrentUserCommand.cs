using System.Threading.Tasks;
using InternetTechLab1.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class GetCurrentUserCommand : GetGitHubInformationCommandBase
{
    public GetCurrentUserCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService, ILoggerService logger)
    : base(gitHubService, visualizerService, logger) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        GitHubUser? gitHubUser = await _service.GetAndSaveUserAsync(username);

        if (gitHubUser != null) 
        {
            _visualizer.ShowGitHubUser(gitHubUser);
        } 
        else
        {
            Console.WriteLine($"Пользователь {username} не найден");
        }
    }
}
