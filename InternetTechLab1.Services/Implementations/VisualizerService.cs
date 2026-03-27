using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetTechLab1.Helpers;
using InternetTechLab1.Models;
using static System.Console;

namespace InternetTechLab1.Services;

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
            Console.WriteLine($"\x1b[31mВ базе данных нет записей типа {typeof(T).Name}\x1b[0m");
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
        foreach(var elem in results)
        {
            Console.WriteLine(elem);
        }
    }
}
