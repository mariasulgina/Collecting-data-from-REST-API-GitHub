using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class GetFollowersCommand : GetGitHubInformationCommandBase
{
    public GetFollowersCommand(IGitHubApiService api, IRelationalDatabaseService db) 
    : base(api, db) { }

    protected override async Task ExecuteGitHubLogic(string username)
    {
        string endpoint = $"users/{username}/followers";

        List<GitHubUser>? followers = await ApiService.GetFromApiGitHubInformationAsync<List<GitHubUser>>(endpoint);

        if (followers != null && followers.Count > 0)
        {
            DbService.SaveApiGitHubFollowersInformation(followers);
            WatchApiGitHubFollowersInformation(followers, username);
        }
        else
        {
            Console.WriteLine($"У пользователя {username} нет подписчиков или они скрыты");
        }
    }

    private void WatchApiGitHubFollowersInformation(List<GitHubUser> followers, string username)
    {
        const string Bold = "\x1b[1m";
        const string Reset = "\x1b[0m";

        Console.WriteLine("\n" + new string('-', 50));
        Console.WriteLine($"{Bold}Подписчики пользователя: {username}{Reset}");
        Console.WriteLine($"Всего найдено: {followers.Count}");
        Console.WriteLine(new string('-', 50));

        foreach (var follower in followers)
        {
            Console.WriteLine($"   {follower.Login}");
            Console.WriteLine($"   Ссылка: {follower.HtmlUrl}");
            Console.WriteLine(new string('-', 30));
        }

        Console.WriteLine(new string('-', 50));
    }
}
