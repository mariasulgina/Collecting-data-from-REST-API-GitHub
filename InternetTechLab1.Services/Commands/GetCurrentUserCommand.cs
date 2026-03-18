using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class GetCurrentUserCommand : GetGitHubInformationCommandBase
{
    public GetCurrentUserCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService, 
    IVisualizerService visualizerService)
    : base(gitHubApiService, relationalDatabaseService, visualizerService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        string endpoint = $"users/{username}";

        GitHubUser? gitHubUser = await ApiService.GetFromApiGitHubInformationAsync<GitHubUser>(endpoint);

        if (gitHubUser != null) 
        {
            DbService.SaveApiGitHubUserInformation(gitHubUser); 
            DbService.ShowAllUsers();
            Visualizer.ShowGitHubUser(gitHubUser);
        } 
        else
        {
            Console.WriteLine($"Пользователь {username} не найден");
        }
    }
}
