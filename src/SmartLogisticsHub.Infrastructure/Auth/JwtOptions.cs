namespace SmartLogisticsHub.Infrastructure.Auth;

public class JwtOptions
{
    public string Issuer { get; set; } = "smarthub";
    public string Audience { get; set; } = "smarthub";
    public string Key { get; set; } = "12345678123456781234567812345678";
    public int ExpMinutes { get; set; } = 60;
}