using MyMoney.Domain.Validation;

namespace MyMoney.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    public Faker Faker { get; set; } = new Faker();

    [Fact]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.NotNullAttribute(value, fieldName);
        action.ShouldNotThrow();
    }

    [Fact]
    public void NotNullThrowWhenNull()
    {
        string? value = null;
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.NotNullAttribute(value, fieldName);
        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe($"{fieldName} should not be null.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.NotNullOrEmptyAttribute(target!, fieldName);
        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe($"{fieldName} should not be null or empty.");
    }

    [Fact]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNullOrEmptyAttribute(target, fieldName);
        action.ShouldNotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.MinLengthAttribute(target, minLength, fieldName);

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe($"{fieldName} should be at least {minLength} characters long.");
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.MinLengthAttribute(target, minLength, fieldName);

        action.ShouldNotThrow();
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.MaxLengthAttribute(target, maxLength, fieldName);

        action
            .ShouldThrow<EntityValidationException>()
            .Message.ShouldBe(
                $"{fieldName} should be less or equal to {maxLength} characters long."
            );
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.MaxLengthAttribute(target, maxLength, fieldName);

        action.ShouldNotThrow();
    }

    public static IEnumerable<object[]> GetValuesLessThanMin(int numberOfTests = 5)
    {
        var faker = new Faker();
        yield return new object[] { "123456", 10 };
        for (var i = 0; i < (numberOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
    {
        var faker = new Faker();
        yield return new object[] { "123456", 6 };
        for (var i = 0; i < (numberOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 5)
    {
        var faker = new Faker();
        yield return new object[] { "123456", 5 };
        for (var i = 0; i < (numberOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, 20);
            yield return new object[] { example, maxLength };
        }
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
    {
        var faker = new Faker();
        yield return new object[] { "123456", 10 };
        for (var i = 0; i < (numberOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }
}
