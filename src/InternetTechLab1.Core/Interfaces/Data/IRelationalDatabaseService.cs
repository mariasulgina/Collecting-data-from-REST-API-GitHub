using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface IRelationalDatabaseService : IDatabaseService
{
    Task SaveApiGitHubUserInformationAsync(GitHubUser user);
    Task SaveApiGitHubReposInformationAsync(List<GitHubRepo> repos);
    Task SaveApiGitHubFollowersInformationAsync(List<GitHubUser> followers);
    Task<List<GitHubRepo>> GetAllReposAsync();
    Task<List<GitHubUser>> GetAllUsersAsync();
    Task<GitHubUser?> GetUserByLoginAsync(string username);
    Task<List<GitHubRepo>?> GetReposByLoginUserAsync(string username);
}
