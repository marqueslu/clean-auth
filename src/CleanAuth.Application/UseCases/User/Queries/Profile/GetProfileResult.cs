namespace CleanAuth.Application.UseCases.User.Queries.Profile;

public record GetProfileResult(Guid Id, string Name, string Email);