using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetTechLab1.Data.Models;

public class GitHubRepoEntity
{
    [Key]
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

    public int? OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    public GitHubUserEntity? Owner { get; set; }
}
