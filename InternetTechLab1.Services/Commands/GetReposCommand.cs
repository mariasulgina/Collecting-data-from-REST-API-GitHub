using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class GetReposCommand : GetGitHubInformationCommandBase
{
    public GetReposCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService, 
    IVisualizerService visualizerService)
    : base(gitHubApiService, relationalDatabaseService, visualizerService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        string endpoint = $"users/{username}/repos";

        List<GitHubRepo>? gitHubRepos = await ApiService.GetFromApiGitHubInformationAsync<List<GitHubRepo>>(endpoint);

        if (gitHubRepos != null && gitHubRepos.Count != 0)
        {
            DbService.SaveApiGitHubReposInformation(gitHubRepos);
            Visualizer.ShowGitHubUser(gitHubRepos);
        } else
        {
            Console.WriteLine($"У пользователя {username} репозитории не найдены");
        }
    }
}
