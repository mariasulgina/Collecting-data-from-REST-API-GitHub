using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class GetReposCommand : GetGitHubInformationCommandBase
{
    public GetReposCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService, ILoggerService logger)
    : base(gitHubService, visualizerService, logger) { }

    protected override async Task ExecuteGitHubLogicAsync(string username)
    {
        await _logger.WriteLogToFileAsync($"[UI] Запрос списка репозиториев для пользователя: {username}");

        List<GitHubRepo>? gitHubRepos = await _service.GetAndSaveReposAsync(username);

        if (gitHubRepos != null && gitHubRepos.Count != 0)
        {
            await _logger.WriteLogToFileAsync($"[UI] Успешно получено {gitHubRepos.Count} репозиториев для {username} и отправлены на визуализацию");

            _visualizer.ShowGitHubRepos(gitHubRepos);
        } else
        {
            await _logger.WriteLogToFileAsync($"[UI] Репозитории для {username} не найдены или API вернул пустой список");

            Console.WriteLine($"У пользователя {username} репозитории не найдены");
        }
    }
}
