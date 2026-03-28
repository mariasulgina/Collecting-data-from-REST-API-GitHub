using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public interface IGitHubApiService
{
    Task<T?> GetFromApiGitHubInformationAsync<T>(string endpoint);
}
