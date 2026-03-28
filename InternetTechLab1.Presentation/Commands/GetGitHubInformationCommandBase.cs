using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;
using InternetTechLab1.Services;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public abstract class GetGitHubInformationCommandBase : ICommand
{
    protected readonly IGitHubService Service;
    protected readonly IVisualizerService Visualizer;

    protected GetGitHubInformationCommandBase(IGitHubService service, IVisualizerService visualizer)
    {
        Service = service;
        Visualizer = visualizer;
    }

    public async Task Execute()
    {
        Console.Write("Введите имя пользователя GitHub: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Имя пользователя не может быть пустым");
        } else
        {
            await ExecuteGitHubLogic(username);
        }
    }

    protected abstract Task ExecuteGitHubLogic(string username);
}
