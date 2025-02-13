
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

WORKDIR /src
COPY ["src/MyMoney.Api/MyMoney.Api.csproj", "src/MyMoney.Api/"]
RUN dotnet restore "src/MyMoney.Api/MyMoney.Api.csproj"

COPY . .
WORKDIR "/src/src/MyMoney.Api"
RUN dotnet build "MyMoney.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyMoney.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyMoney.Api.dll"]