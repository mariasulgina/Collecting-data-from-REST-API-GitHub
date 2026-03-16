using InternetTechLab1.Services;
using InternetTechLab1.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetTechLab1.Data.Rdbms;

public class GitHubRepository : IRelationalDatabaseService
{
    public void SaveApiGitHubUserInformation(GitHubUser user) 
    {

    }

    public void SaveApiGitHubReposInformation(List<GitHubRepo> repos)
    {

    }

    public void SaveApiGitHubFollowersInformation(List<GitHubUser> followers)
    {

    }

    public void ShowAllUsers()
    {

    }
}
