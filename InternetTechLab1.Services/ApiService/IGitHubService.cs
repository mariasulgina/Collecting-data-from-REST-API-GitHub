using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public interface IGitHubService
{
    Task<GitHubUser?> GetAndSaveUserAsync(string username);
    Task<List<GitHubUser>?> GetAndSaveFollowersAsync(string username);
    Task<List<GitHubRepo>?> GetAndSaveReposAsync(string username);
}