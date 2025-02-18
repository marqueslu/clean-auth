
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

WORKDIR /src
COPY ["src/CleanAuth.Api/CleanAuth.Api.csproj", "src/CleanAuth.Api/"]
RUN dotnet restore "src/CleanAuth.Api/CleanAuth.Api.csproj"

COPY . .
WORKDIR "/src/src/CleanAuth.Api"
RUN dotnet build "CleanAuth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanAuth.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanAuth.Api.dll"]