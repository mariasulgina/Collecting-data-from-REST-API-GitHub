using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class GetCurrentUserCommand : GetGitHubInformationCommandBase
{
    public GetCurrentUserCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService)
    : base(gitHubApiService, relationalDatabaseService) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        string endpoint = $"users/{username}";

        GitHubUser? gitHubUser = await ApiService.GetFromApiGitHubInformationAsync<GitHubUser>(endpoint);

        if (gitHubUser != null) 
        {
            DbService.SaveApiGitHubUserInformation(gitHubUser); 
            DbService.ShowAllUsers();
            WatchApiGitHubUserInformation(gitHubUser);
        } 
        else
        {
            Console.WriteLine($"Пользователь {username} не найден");
        }
    }

    private void WatchApiGitHubUserInformation(GitHubUser user) 
    {
        const string Bold = "\x1b[1m";
        const string Reset = "\x1b[0m";

        Console.WriteLine("\n" + new string('-', 50));
        Console.WriteLine($"{Bold}Профиль пользователя: {user.Login}{Reset}");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine($" Имя: {user.Name ?? "Не указано"}");
        Console.WriteLine($" ID: {user.Id}");
        Console.WriteLine($" Местоположение: {user.Location ?? "Скрыто"}");
        Console.WriteLine($" Био: {(string.IsNullOrEmpty(user.Bio) ? "Информация отсутствует" : user.Bio)}");
        Console.WriteLine($" Публичные репозитории: {user.PublicRepos}");
        Console.WriteLine($" Подписчики: {user.Followers} | Подписки: {user.Following}");
        Console.WriteLine($" GitHub URL: {user.HtmlUrl}");

        Console.WriteLine(new string('-', 50));
    }
}
