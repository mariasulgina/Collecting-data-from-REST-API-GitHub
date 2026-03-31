using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface IGitHubService
{
    Task<GitHubUser?> GetAndSaveUserAsync(string username);
    Task<List<GitHubUser>?> GetAndSaveFollowersAsync(string username);
    Task<List<GitHubRepo>?> GetAndSaveReposAsync(string username);
    Task<GitHubUser?> GetUserByLoginAsync(string username);
    Task<List<GitHubRepo>?> GetReposByLoginUserAsync(string username);
}