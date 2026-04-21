using System.Text.Json.Serialization;

namespace InternetTechLab1.Core.Models;

public class GitHubRepo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public string? Description { get; set; }
    public string? HtmlUrl { get; set; } 
    public bool IsPrivate { get; set; }
    public string? Language { get; set; }
    public int StargazersCount { get; set; }
    public int ForksCount { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public GitHubUser? Owner { get; set; }
}
