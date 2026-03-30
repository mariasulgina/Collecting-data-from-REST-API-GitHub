using InternetTechLab1.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface IRelationalDatabaseService : IDatabaseService
{
    Task SaveApiGitHubUserInformation(GitHubUser user);
    Task SaveApiGitHubReposInformation(List<GitHubRepo> repos);
    Task SaveApiGitHubFollowersInformation(List<GitHubUser> followers);
    Task<List<GitHubRepo>> GetAllRepos();
    Task<List<GitHubUser>> GetAllUsers();
}
