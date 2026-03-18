using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetTechLab1.Services;

public abstract class GetGitHubInformationCommandBase : ICommand
{
    protected readonly IGitHubApiService ApiService;
    protected readonly IRelationalDatabaseService DbService;
    protected readonly IVisualizerService Visualizer;

    protected GetGitHubInformationCommandBase(IGitHubApiService apiService, IRelationalDatabaseService dbService, IVisualizerService visualizer)
    {
        ApiService = apiService;
        DbService = dbService;
        Visualizer = visualizer;
    }

    public async Task Execute()
    {
        Console.Write("Введите имя пользователя GitHub: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Имя пользователя не может быть пустым");
            return;
        }

        await ExecuteGitHubLogic(username);
    }

    protected abstract Task ExecuteGitHubLogic(string username);
}
