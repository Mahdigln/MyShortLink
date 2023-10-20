using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortLink.Application.Interfaces;
using ShortLink.Application.Services;
using ShortLink.Domain.Interface;
using ShortLink.Infra.Data.Repositories;

namespace ShortLink.Infra.IoC;

public static class DependencyContainer
{
    public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        #region repository
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILinkRepository, LinkRepository>();
        #endregion

        #region service
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILinkService, LinkService>();
        #endregion

        #region tools
        services.AddScoped<IPasswordHelper, PasswordHelper>();

        #endregion


        return services;
    }
}