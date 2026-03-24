using InternetTechLab1.Services;

namespace InternetTechLab1.Data.NoSql;

public class ScrapingRepository : INonRelationalDatabaseService
{
    private readonly NoSqlSettings _noSqlSettings = new();
    
    public async Task ClearDataBase()
    {
        
    }
}
