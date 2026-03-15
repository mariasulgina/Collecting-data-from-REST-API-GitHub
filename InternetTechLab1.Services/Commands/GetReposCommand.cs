using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class GetReposCommand : GetGitHubInformationCommandBase
{
    public GetReposCommand(IGitHubApiService gitHubApiService, IRelationalDatabaseService relationalDatabaseService)
    : base(gitHubApiService, relationalDatabaseService) {} 

    protected override async Task ExecuteGitHubLogic(string username)
    {
        string endpoint = $"users/{username}/repos";

        List<GitHubRepo>? gitHubRepos = await ApiService.GetFromApiGitHubInformationAsync<List<GitHubRepo>>(endpoint);

        if (gitHubRepos != null && gitHubRepos.Count != 0)
        {
            DbService.SaveApiGitHubReposInformation(gitHubRepos);
            WatchApiGitHubReposInformation(gitHubRepos);
        } else
        {
            Console.WriteLine($"У пользователя {username} репозитории не найдены");
        }
    }

    private void WatchApiGitHubReposInformation(List<GitHubRepo> gitHubRepos) 
    {
        Console.WriteLine("\n" + new string(' ', 50));
        Console.WriteLine($"Результат поиска: Найдено {gitHubRepos.Count} репозиториев");
        Console.WriteLine(new string(' ', 50));

        foreach (var repo in gitHubRepos)
        {
            Console.WriteLine($"   Репозиторий: {repo.Name}");
            Console.WriteLine($"   URL: {repo.HtmlUrl}");
            Console.WriteLine($"   Звезды: {repo.StargazersCount,-5} | 🍴 Форки: {repo.ForksCount}");
            Console.WriteLine($"   Описание: {(string.IsNullOrEmpty(repo.Description) ? "Нет описания" : repo.Description)}");
            Console.WriteLine($"   Язык: {repo.Language ?? "Не определен"}");
            Console.WriteLine(new string('-', 30));
        }

        Console.WriteLine(new string(' ', 50));
    }
}
