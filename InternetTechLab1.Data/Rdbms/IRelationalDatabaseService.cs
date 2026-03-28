using InternetTechLab1.Models;

namespace InternetTechLab1.Data;

public interface IRelationalDatabaseService : IDatabaseService
{
    void SaveApiGitHubUserInformation(GitHubUser user);
    void SaveApiGitHubReposInformation(List<GitHubRepo> repos);
    void SaveApiGitHubFollowersInformation(List<GitHubUser> followers);
    List<GitHubRepo> GetAllRepos();
    List<GitHubUser> GetAllUsers();
}
