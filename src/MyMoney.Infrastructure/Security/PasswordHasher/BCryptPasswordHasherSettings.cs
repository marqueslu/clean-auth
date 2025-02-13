namespace MyMoney.Infrastructure.Security.PasswordHasher;

public class BCryptPasswordHasherSettings
{
    public const string Section = "BCryptPasswordHasherSettings";
    public int WorkFactor { get; set; }
}
