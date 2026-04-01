using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class GetFollowersCommand : GetGitHubInformationCommandBase
{
    public GetFollowersCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService, ILoggerService logger)
    : base(gitHubService, visualizerService, logger) { }

    protected override async Task ExecuteGitHubLogicAsync(string username)
    {
        await _logger.WriteLogToFileAsync($"[UI] Запрос списка подписчиков для: {username}");

        List<GitHubUser>? followers = await _service.GetAndSaveFollowersAsync(username);

        if (followers != null && followers.Count > 0)
        {
            await _logger.WriteLogToFileAsync($"[UI] Успешно получено {followers.Count} подписчиков для {username} и отправлены на визуализацию");

            _visualizer.ShowGitHubFollowers(followers, username);
        }
        else
        {
            await _logger.WriteLogToFileAsync($"[UI] У пользователя {username} список подписчиков пуст или скрыт настройками приватности");

            Console.WriteLine($"У пользователя {username} нет подписчиков или они скрыты");
        }
    }
}
