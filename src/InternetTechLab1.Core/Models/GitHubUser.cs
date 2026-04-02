using System.Text.Json.Serialization;

namespace InternetTechLab1.Core.Models;

public class GitHubUser
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? AvatarUrl { get; set; }
    public string? HtmlUrl { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public int PublicRepos { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public DateTime? CreatedAt { get; set; } 
    public string? Email { get; set; }

    public List<GitHubUser> FollowersList { get; set; } = new();
}
