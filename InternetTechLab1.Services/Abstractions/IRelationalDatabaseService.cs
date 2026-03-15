using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public interface IRelationalDatabaseService : IDatabaseService
{
    void SaveApiGitHubUserInformation(GitHubUser user);
    void SaveApiGitHubReposInformation(List<GitHubRepo> repos);
    void SaveApiGitHubFollowersInformation(List<GitHubUser> followers);
}
