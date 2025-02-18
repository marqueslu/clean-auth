namespace CleanAuth.Application.Intefaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid id, string name, string email);
}
