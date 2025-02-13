namespace MyMoney.IntegrationTests.Application.UseCases.Authentication.Commands.SignUp;

public class SignUpTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 15)
    {
        var fixture = new SignUpTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 5;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add([
                        fixture.GetCommandWithTooLongName(),
                        "Name should be less or equal to 100 characters long."
                    ]);
                    break;
                case 1:
                    invalidInputsList.Add([
                        fixture.GetCommandWithTooShortName(),
                        "Name should be at least 5 characters long."
                    ]);
                    break;
                case 2:
                    invalidInputsList.Add([
                        fixture.GetCommandWithTooShortPassword(),
                        "Password should be at least 8 characters long."
                    ]);
                    break;
                case 3:
                    invalidInputsList.Add([
                        fixture.GetCommandWithNullName(),
                        "Name should not be null or empty."
                    ]);
                    break;
                case 4:
                    invalidInputsList.Add([
                        fixture.GetCommandWithInvalidEmail(),
                        "Email is not a valid email address."
                    ]);
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }
}