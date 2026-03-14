using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetTechLab1.Services;

public class WebScrapingCommand : ICommand
{
    public WebScrapingCommand(IGitHubScrapingService gitHubScrapingService, INonRelationalDatabaseService nonRelationalDatabaseService)
    {

    }

    public void Execute() 
    {
        
    }
}
