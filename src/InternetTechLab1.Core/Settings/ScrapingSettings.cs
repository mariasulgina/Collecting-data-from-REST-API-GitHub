namespace InternetTechLab1.Core.Settings;

public class ScrapingSettings
{
    public string UserAgent { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; }
    public int MaxBodyLength { get; set; }
}
