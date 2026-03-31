namespace InternetTechLab1.Core.Interfaces;

public interface IGitHubApiService
{
    Task<T?> GetFromApiGitHubInformationAsync<T>(string endpoint);
}
