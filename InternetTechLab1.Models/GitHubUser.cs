using System.Text.Json.Serialization;

namespace InternetTechLab1.Models;

public class GitHubUser
{
    //и для репозитория и для пользователя
    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    //только для пользователя
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("bio")]
    public string? Bio { get; set; }

    [JsonPropertyName("public_repos")]
    public int PublicRepos { get; set; }

    [JsonPropertyName("followers")]
    public int Followers { get; set; }

    [JsonPropertyName("following")]
    public int Following { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; } 

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

//https://api.github.com/users/{username}
//https://api.github.com/users/{username}/repos
//https://api.github.com/users/{username}/followers
