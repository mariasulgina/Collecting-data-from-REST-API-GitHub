using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;
using InternetTechLab1.Services;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public class GetReposCommand : GetGitHubInformationCommandBase
{
    public GetReposCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService)
    : base(gitHubService, visualizerService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        List<GitHubRepo>? gitHubRepos = await Service.GetAndSaveReposAsync(username);

        if (gitHubRepos != null && gitHubRepos.Count != 0)
        {
            Visualizer.ShowGitHubRepos(gitHubRepos);
        } else
        {
            Console.WriteLine($"У пользователя {username} репозитории не найдены");
        }
    }
}
