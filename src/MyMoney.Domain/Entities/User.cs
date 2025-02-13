using MyMoney.Domain.Interfaces.Security;
using MyMoney.Domain.Validation;

namespace MyMoney.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public User() { }

    public User(string name, string email, string password, IPasswordHasher passwordHasher)
    {
        Name = name;
        Email = email;
        Password = password;
        Validate();
        Password = passwordHasher.HashPassword(password);
    }

    public void UpdateName(string name)
    {
        Name = name;
        Validate();
        SetLastUpdate();
    }

    public void UpdatePassword(string password, IPasswordHasher passwordHasher)
    {
        Password = password;
        Validate();
        Password = passwordHasher.HashPassword(password);
        SetLastUpdate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmptyAttribute(Name, nameof(Name));
        DomainValidation.NotNullOrEmptyAttribute(Email, nameof(Email));
        DomainValidation.NotNullOrEmptyAttribute(Password, nameof(Password));
        DomainValidation.ValidEmailAttribute(Email, nameof(Email));
        DomainValidation.MinLengthAttribute(Password, 8, nameof(Password));
        DomainValidation.MinLengthAttribute(Name, 5, nameof(Name));
        DomainValidation.MaxLengthAttribute(Name, 100, nameof(Name));
        DomainValidation.MaxLengthAttribute(Email, 255, nameof(Email));
    }
}
