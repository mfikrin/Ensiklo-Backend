FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.sln .
COPY PPL/*.csproj ./PPL/
RUN dotnet restore

COPY PPL/. ./PPL/
WORKDIR /source/PPL
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "PPL.dll"]