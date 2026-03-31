using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetTechLab1.Data.Models;

public class GitHubUserEntity
{
    [Key]
    public int Id { get; set; }

    public string? Login { get; set; }
    public string? AvatarUrl { get; set; }
    public string? HtmlUrl { get; set; }
    public string? Name { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public int PublicRepos { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public DateTime? CreatedAt { get; set; } 
    public string? Email { get; set; }

    public List<GitHubRepoEntity> Repositories { get; set; } = new();
    public List<GitHubUserEntity> FollowersList { get; set; } = new();
}
