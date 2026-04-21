using InternetTechLab1.Core.Models;
using InternetTechLab1.Services.Models;
using InternetTechLab1.Core.Interfaces;


namespace InternetTechLab1.Services.Mappers;

public static class GitHubMapper
{ 
    /// <summary>
    /// Преобразует объект передачи данных (DTO) в доменную модель пользователя.
    /// </summary>
    public static GitHubUser MapToDomainUser(this GitHubUserDto dto) => new()
    {
        Id = dto.Id,
        Login = dto.Login,
        Name = dto.Name,
        AvatarUrl = dto.AvatarUrl,
        Bio = dto.Bio,
        Location = dto.Location,
        Company = dto.Company,
        PublicRepos = dto.PublicRepos,
        Followers = dto.Followers,
        Following = dto.Following,
        CreatedAt = dto.CreatedAt,
        Email = dto.Email
    };

    /// <summary>
    /// Преобразует объект передачи данных (DTO) в доменную модель репозитория.
    /// </summary>
    public static GitHubRepo MapToDomainRepo(this GitHubRepoDto dto) 
    {
        return new GitHubRepo 
        {
            Id = dto.Id,
            Name = dto.Name,
            FullName = dto.FullName,
            Description = dto.Description,
            Language = dto.Language,
            StargazersCount = dto.StargazersCount,
            ForksCount = dto.ForksCount,
            Owner = dto.Owner != null ? MapToDomainUser(dto.Owner) : null 
        };
    }
}
