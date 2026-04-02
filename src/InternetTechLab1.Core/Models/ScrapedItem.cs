namespace InternetTechLab1.Core.Models;

public record ScrapedItem(
    string Url,
    string DataType,
    string Value
)
{
    public ScrapedItem() : this(string.Empty, "Unknown", string.Empty) { }
}
