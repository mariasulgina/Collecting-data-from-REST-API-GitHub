using InternetTechLab1.Models;
using InternetTechLab1.Data;

namespace InternetTechLab1.Services;

public class GitHubService : IGitHubService
{
    private readonly IGitHubApiService _apiClient;
    private readonly IRelationalDatabaseService _database;

    public GitHubService(IGitHubApiService apiClient, IRelationalDatabaseService database)
    {
        _apiClient = apiClient;
        _database = database;
    }

    public async Task<GitHubUser?> GetAndSaveUserAsync(string username)
    {
        string endpoint = $"users/{username}";

        var user = await _apiClient.GetFromApiGitHubInformationAsync<GitHubUser>(endpoint);

        if (user != null)
        {
            _database.SaveApiGitHubUserInformation(user);
        }

        return user;
    }

    public async Task<List<GitHubUser>?> GetAndSaveFollowersAsync(string username)
    {
        string endpoint = $"users/{username}/followers";
        
        var followers = await _apiClient.GetFromApiGitHubInformationAsync<List<GitHubUser>>(endpoint);

        if (followers != null && followers.Count > 0)
        {
            _database.SaveApiGitHubFollowersInformation(followers);
        }

        return followers;
    }

    public async Task<List<GitHubRepo>?> GetAndSaveReposAsync(string username)
    {
        string endpoint = $"users/{username}/repos";
        var repos = await _apiClient.GetFromApiGitHubInformationAsync<List<GitHubRepo>>(endpoint);

        if (repos != null && repos.Count > 0)
        {
            _database.SaveApiGitHubReposInformation(repos);
        }

        return repos;
    }
}
