namespace InternetTechLab1.Core.Interfaces;

public interface ILoggerService 
{
    Task WriteLogToFileAsync(string message);
}
