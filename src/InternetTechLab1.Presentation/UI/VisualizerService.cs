using System.Collections.Generic;
using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using static System.Console;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.UI;

/// <summary>
/// Сервис для красивой визуализации данных в консольном интерфейсе.
/// </summary>
public class VisualizerService : IVisualizerService
{
    /// <summary>
    /// Отображает детальную информацию о конкретном пользователе GitHub.
    /// </summary>
    public void ShowGitHubUser(GitHubUser user) 
    {
        Clear();
        ConsoleStyler.PrintHeader("Профиль пользователя:");

        ShowGitHubUserInformation(user);
        ConsoleStyler.PrintIndentation();
    }

    /// <summary>
    /// Вспомогательный метод для вывода текстовых полей профиля пользователя.
    /// </summary>
    private void ShowGitHubUserInformation(GitHubUser user) 
    {
        Console.WriteLine($" Имя: {user.Name ?? "Не указано"}");
        Console.WriteLine($" ID: {user.Id}");
        Console.WriteLine($" Местоположение: {user.Location ?? "Скрыто"}");
        Console.WriteLine($" Био: {(string.IsNullOrEmpty(user.Bio) ? "Информация отсутствует" : user.Bio)}");
        Console.WriteLine($" Публичные репозитории: {user.PublicRepos}");
        Console.WriteLine($" Подписчики: {user.Followers} | Подписки: {user.Following}");
        Console.WriteLine($" GitHub URL: {user.HtmlUrl}");
    }

    /// <summary>
    /// Выводит список подписчиков указанного пользователя.
    /// </summary>
    public void ShowGitHubFollowers(List<GitHubUser> followers, string username)
    {
        Clear();
        ConsoleStyler.PrintHeader("Подписчики пользователя:");
        Console.WriteLine($"Всего найдено: {followers.Count}");

        foreach (var follower in followers)
        {
            Console.WriteLine($"   {follower.Login}");
            Console.WriteLine($"   Ссылка: {follower.HtmlUrl}");
            ConsoleStyler.PrintIndentation();
        }

        ConsoleStyler.PrintIndentation();
    }

    /// <summary>
    /// Отображает список репозиториев GitHub.
    /// </summary>
    public void ShowGitHubRepos(List<GitHubRepo> repos)
    {
        Clear();
        ConsoleStyler.PrintHeader("Репозитории пользователя:");
        Console.WriteLine($"Результат поиска: Найдено {repos.Count} репозиториев");

        ShowGitHubReposInformation(repos);
        ConsoleStyler.PrintIndentation();
    }

    /// <summary>
    /// Вспомогательный метод для циклического вывода информации о каждом репозитории.
    /// </summary>
    private void ShowGitHubReposInformation(List<GitHubRepo> repos) 
    {
        foreach (var repo in repos)
        {
            Console.WriteLine($"   Репозиторий: {repo.Name}");
            Console.WriteLine($"   URL: {repo.HtmlUrl}");
            Console.WriteLine($"   Звезды: {repo.StargazersCount,-5} | Форки: {repo.ForksCount}");
            Console.WriteLine($"   Описание: {(string.IsNullOrEmpty(repo.Description) ? "Нет описания" : repo.Description)}");
            Console.WriteLine($"   Язык: {repo.Language ?? "Не определен"}");
            ConsoleStyler.PrintIndentation();
        }
    }

    /// <summary>
    /// Универсальный метод для отображения данных из базы данных с использованием рефлексии, 
    /// то есть вытаскиваем нужные свойства из объектов, чтобы узнать, что за класс перед нами.
    /// Выводит только строковые и целочисленные свойства объектов.
    /// </summary>
    public void ShowDb<T>(IEnumerable<T> data) where T : class
    {
        var dataList = data.ToList();

        if (dataList.Count == 0) 
        {
            Console.WriteLine($"В базе данных нет записей типа {typeof(T).Name}");
        } else
        {
            ConsoleStyler.PrintHeader($"Список: {typeof(T).Name}");
        
            var properties = typeof(T).GetProperties();

            foreach(var elem in dataList)
            {
                string dataForElem = "";
                foreach(var property in properties)
                {
                    if (property.PropertyType == typeof(string) || property.PropertyType == typeof(int))
                    {
                        dataForElem += $"{ConsoleStyler.Bold}{property.Name}{ConsoleStyler.Reset}: {property.GetValue(elem) ?? "null"} | ";
                    }
                }
                Console.WriteLine($"  -> {dataForElem}");
                ConsoleStyler.PrintIndentation();
            }
        }
    }

    /// <summary>
    /// Выводит результаты веб-скрапинга, сгруппированные по типу данных.
    /// </summary>
    public void ShowScrapeResults(IEnumerable<ScrapedItem> results)
    {
        if (results == null || !results.Any())
        {
            Console.WriteLine("Результаты скрапинга отсутствуют");
            return;
        } 

        ConsoleStyler.PrintHeader($"Результаты скреппинга");

        var groupResults = results.GroupBy(p => p.DataType);
        foreach(var group in groupResults)
        {
            Console.WriteLine($"\n{ConsoleStyler.Bold}{ConsoleStyler.Cyan} {group.Key} {ConsoleStyler.Reset}");
            foreach (var item in group)
            {
                Console.WriteLine($"  > {item.Value}");
            }
            ConsoleStyler.PrintIndentation();
        }
    }

    /// <summary>
    /// Отображает данные из NoSQL базы данных (использует логику отображения результатов скрапинга).
    /// </summary>
    public void ShowNoSqlDb(IEnumerable<ScrapedItem> results)
    {
        ShowScrapeResults(results);
    }

    /// <summary>
    /// Комплексный метод для одновременного отображения профиля пользователя и его репозиториев.
    /// </summary>
    public void ShowGitHubUserAndHisRepos(GitHubUser user, List<GitHubRepo> repos)
    {
        Clear();
        ConsoleStyler.PrintHeader("Профиль пользователя и его репозитории:");
        ConsoleStyler.PrintHeader("Профиль пользователя:");

        ShowGitHubUserInformation(user);
        ConsoleStyler.PrintIndentation();

        ConsoleStyler.PrintHeader("Репозитории пользователя:");
        Console.WriteLine($"Результат поиска: Найдено {repos.Count} репозиториев");

        ShowGitHubReposInformation(repos);
        ConsoleStyler.PrintIndentation();
    }
}
