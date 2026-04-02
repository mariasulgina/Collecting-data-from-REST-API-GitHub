using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class SearchCommand : GetGitHubInformationCommandBase
{
    public SearchCommand(IGitHubService gitHubService, 
    IVisualizerService visualizerService, ILoggerService logger)
    : base(gitHubService, visualizerService, logger) { }

    protected override async Task ExecuteGitHubLogicAsync(string username)
    {
        await _logger.WriteLogToFileAsync($"[UI] Запрос пользователя {username} и списка его репозиториев");

        var user = await _service.GetUserByLoginAsync(username);
        var repos = await _service.GetReposByLoginUserAsync(username);

        if (user != null && (repos != null && repos.Any()))
        {
            await _logger.WriteLogToFileAsync($"[UI] Успех: Для {username} найден профиль и {repos.Count} репозиториев");
            _visualizer.ShowGitHubUserAndHisRepos(user, repos);
        }
        else if (user != null)
        {
            await _logger.WriteLogToFileAsync($"[UI] Частичный успех: Для {username} найден только профиль. Репозитории отсутствуют");
            Console.WriteLine($"Получены данные о пользователе '{username}'.\nУ Пользователя '{username}' еще нет репозиториев");

            _visualizer.ShowGitHubUser(user);
        }
        else if (repos != null && repos.Any())
        {
            await _logger.WriteLogToFileAsync($"[Warning] Странное состояние: Для {username} найдены репозитории ({repos.Count}), но сам профиль не найден");
            Console.WriteLine($"Получены данные о репозитории пользователя '{username}'.\nПользователя '{username}' еще нет в базе. Сначала используйте команду 'Получить данные о пользователе'");

            _visualizer.ShowGitHubRepos(repos);
        }
        else
        {
            await _logger.WriteLogToFileAsync($"[UI] Поиск завершен: Данные для {username} в локальной базе полностью отсутствуют");
            Console.WriteLine($"Пользователя '{username}' еще нет в базе.\nУ Пользователя '{username}' еще нет репозиториев");
        }
    }
}
