using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class GetCurrentUserCommand : GetGitHubInformationCommandBase
{
    public GetCurrentUserCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService, ILoggerService logger)
    : base(gitHubService, visualizerService, logger) { }

    protected override async Task ExecuteGitHubLogicAsync(string username)
    {
        GitHubUser? gitHubUser = await _service.GetAndSaveUserAsync(username);

        if (gitHubUser != null) 
        {
            await _logger.WriteLogToFileAsync($"[UI] Данные пользователя {username} (ID: {gitHubUser.Id}) успешно получены и отправлены на визуализацию");

            _visualizer.ShowGitHubUser(gitHubUser);
        } 
        else
        {
            await _logger.WriteLogToFileAsync($"[UI] Предупреждение: Пользователь {username} не найден в GitHub API");

            Console.WriteLine($"Пользователь {username} не найден");
        }
    }
}
