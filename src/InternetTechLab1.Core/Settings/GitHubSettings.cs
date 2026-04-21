namespace InternetTechLab1.Core.Settings;

public class GitHubSettings
{
    public string UserAgent { get; set; } = string.Empty;
    public string AcceptHeader { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
}
