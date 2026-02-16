namespace SmartLogisticsHub.Infrastructure.Auth;

public interface IJwtTokenService
{
    string CreateToken(AppUser user, IList<string> roles);
}