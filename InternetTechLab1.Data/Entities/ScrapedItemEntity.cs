using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ScrapedItemEntity
{
    [Key]
    public int Id { get; set; } 
    
    public string Url { get; set; } = string.Empty;
    public string DataType { get; set; } = "Unknown";
    public string Value { get; set; } = string.Empty;
}
