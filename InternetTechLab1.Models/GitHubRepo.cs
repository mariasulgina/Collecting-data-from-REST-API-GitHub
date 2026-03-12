namespace InternetTechLab1.Models;
using System.Text.Json.Serialization; 

public class GitHubRepo
{
    public int id { get; set; }
    public string? name { get; set; } //NameRepository
    public string? full_name { get; set; } //FullNameRepository
    [JsonPropertyName("private")]
    public bool? IsPrivate { get; set; }
    public GitHubUser? owner { get; set; } //owner
}
