using CleanAuth.Api;
using CleanAuth.Api.Configurations;
using CleanAuth.Application;
using CleanAuth.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation().AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.AddHttpLogging(o => { });
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors("CORS");
app.UseHttpLogging();
app.MigrateDatabase();
app.UseDocumentation();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

namespace CleanAuth.Api
{
    public partial class Program { }
}
