using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetTechLab1.Models;

namespace InternetTechLab1.UI;

public interface IVisualizerService
{
    void ShowGitHubUser(GitHubUser user);
    void ShowGitHubFollowers(List<GitHubUser> followers, string username);
    void ShowGitHubRepos(List<GitHubRepo> gitHubRepos);
    void ShowDb<T>(IEnumerable<T> data) where T : class;
    void ShowScrapeResults(IEnumerable<ScrapedItem> results);
    void ShowNoSqlDb(IEnumerable<ScrapedItem> results);
}
