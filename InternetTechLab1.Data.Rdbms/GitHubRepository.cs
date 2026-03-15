using InternetTechLab1.Services;
using InternetTechLab1.Models;

namespace InternetTechLab1.Data.Rdbms;

public class GitHubRepository : IRelationalDatabaseService
{
    //GitHubRepository → БД
    //использует GitHubDbContext
    public void SaveApiGitHubUserInformation(GitHubUser user) 
    {

    }

    public void SaveApiGitHubReposInformation(List<GitHubRepo> repos)
    {

    }

    public void SaveApiGitHubFollowersInformation(List<GitHubUser> followers)
    {

    }
}
