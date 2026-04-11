using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InternetTechLab1.Data.Models;

public class ScrapedItemEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } 
    
    public string Url { get; set; } = string.Empty;
    public string DataType { get; set; } = "Unknown";
    public string Value { get; set; } = string.Empty;
}
