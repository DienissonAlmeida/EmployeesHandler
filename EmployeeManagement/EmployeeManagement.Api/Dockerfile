FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 80

COPY ["EmployeeManagement.Api/EmployeeManagement.Api.csproj", "EmployeeManagement.Api/"]
RUN dotnet restore "EmployeeManagement.Api/EmployeeManagement.Api.csproj"

COPY . .

WORKDIR "/src/EmployeeManagement.Api"
RUN dotnet publish "EmployeeManagement.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "EmployeeManagement.Api.dll"]
