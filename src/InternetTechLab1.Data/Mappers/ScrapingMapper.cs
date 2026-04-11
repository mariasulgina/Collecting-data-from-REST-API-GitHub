using InternetTechLab1.Data.Models;
using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Data.Mappers;

public static class ScrapingMapper
{ 
    public static ScrapedItemEntity MapToEntity(this ScrapedItem item) => new()
    {
        Url = item.Url,
        DataType = item.DataType,
        Value = item.Value
    };

    public static ScrapedItem MapToDomain(this ScrapedItemEntity entity) => new(
        entity.Url,
        entity.DataType,
        entity.Value
    );
}
