using InternetTechLab1.Data.Models;
using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Data.Mappers;

/// <summary>
/// Статический класс, содержащий методы для преобразования данных между доменными моделями GitHub и бд.
/// </summary>
public static class GitHubMapper
{ 
    /// <summary>
    /// Преобразует доменную модель пользователя в сущность бд.
    /// </summary>
    public static GitHubUserEntity MapToEntity(this GitHubUser user) => new()
    {
        Id = user.Id,
        Login = user.Login,
        AvatarUrl = user.AvatarUrl,
        HtmlUrl = user.HtmlUrl,
        Name = user.Name,
        Company = user.Company,
        Location = user.Location,
        Bio = user.Bio,
        PublicRepos = user.PublicRepos,
        Followers = user.Followers,
        Following = user.Following,
        CreatedAt = user.CreatedAt,
        Email = user.Email
    };

    /// <summary>
    /// Преобразует доменную модель репозитория в сущность бд.
    /// </summary>
    public static GitHubRepoEntity MapToEntity(this GitHubRepo repo) => new()
    {
        Id = repo.Id,
        Name = repo.Name,
        FullName = repo.FullName,
        Description = repo.Description,
        HtmlUrl = repo.HtmlUrl,
        IsPrivate = repo.IsPrivate,
        Language = repo.Language,
        StargazersCount = repo.StargazersCount,
        ForksCount = repo.ForksCount,
        CreatedAt = repo.CreatedAt,
        OwnerId = repo.Owner?.Id 
    };

    /// <summary>
    /// Преобразует сущность бд в доменную модель.
    /// </summary>
    public static GitHubUser MapToDomain(this GitHubUserEntity entity) => new()
    {
        Id = entity.Id,
        Login = entity.Login,
        Name = entity.Name,
        AvatarUrl = entity.AvatarUrl,
        Bio = entity.Bio,
        Location = entity.Location,
        Company = entity.Company,
        PublicRepos = entity.PublicRepos,
        Followers = entity.Followers,
        Following = entity.Following,
        CreatedAt = entity.CreatedAt,
        Email = entity.Email
    };

    /// <summary>
    /// Преобразует сущность бд в доменную модель.
    /// Включает в себя рекурсивный маппинг владельца репозитория.
    /// </summary>
    public static GitHubRepo MapToDomain(this GitHubRepoEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        FullName = entity.FullName,
        Description = entity.Description,
        Language = entity.Language,
        StargazersCount = entity.StargazersCount,
        ForksCount = entity.ForksCount,
        Owner = entity.Owner != null ? MapToDomain(entity.Owner) : null
    };
}
