using System.Collections.Generic;
using System.Threading.Tasks;
using InternetTechLab1.Models;
using static System.Console;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.UI;

public class VisualizerService : IVisualizerService
{
    public void ShowGitHubUser(GitHubUser user) 
    {
        Clear();
        ConsoleStyler.PrintHeader("Профиль пользователя:");

        Console.WriteLine($" Имя: {user.Name ?? "Не указано"}");
        Console.WriteLine($" ID: {user.Id}");
        Console.WriteLine($" Местоположение: {user.Location ?? "Скрыто"}");
        Console.WriteLine($" Био: {(string.IsNullOrEmpty(user.Bio) ? "Информация отсутствует" : user.Bio)}");
        Console.WriteLine($" Публичные репозитории: {user.PublicRepos}");
        Console.WriteLine($" Подписчики: {user.Followers} | Подписки: {user.Following}");
        Console.WriteLine($" GitHub URL: {user.HtmlUrl}");

        ConsoleStyler.PrintIndentation();
    }

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

    public void ShowGitHubRepos(List<GitHubRepo> gitHubRepos)
    {
        Clear();
        ConsoleStyler.PrintHeader("Репозитории пользователя:");
        Console.WriteLine($"Результат поиска: Найдено {gitHubRepos.Count} репозиториев");

        foreach (var repo in gitHubRepos)
        {
            Console.WriteLine($"   Репозиторий: {repo.Name}");
            Console.WriteLine($"   URL: {repo.HtmlUrl}");
            Console.WriteLine($"   Звезды: {repo.StargazersCount,-5} | Форки: {repo.ForksCount}");
            Console.WriteLine($"   Описание: {(string.IsNullOrEmpty(repo.Description) ? "Нет описания" : repo.Description)}");
            Console.WriteLine($"   Язык: {repo.Language ?? "Не определен"}");
            ConsoleStyler.PrintIndentation();
        }

        ConsoleStyler.PrintIndentation();
    }

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
                Console.WriteLine($"  → {dataForElem}");
                ConsoleStyler.PrintIndentation();
            }
        }
    }

    public void ShowScrapeResults(IEnumerable<ScrapedItem> results)
    {
        if (results == null || !results.Any())
        {
            Console.WriteLine("Результаты скрапинга отсутствуют");
            return;
        } 

        string firstUrl = results.First().Url;
        ConsoleStyler.PrintHeader($"Результаты для: {firstUrl}");

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

    public void ShowNoSqlDb(IEnumerable<ScrapedItem> results)
    {
        ShowScrapeResults(results);
    }
}
