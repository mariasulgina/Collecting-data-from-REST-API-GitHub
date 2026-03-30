namespace InternetTechLab1.Core.Interfaces;

public interface ILoggerService 
{
    Task WriteLogToFile(string message);
}
