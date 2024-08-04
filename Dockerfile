# Use .NET Runtime image as the base for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 8080

# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /source

# Copy project files and restore dependencies
COPY ./EnglishWordsLearning.sln .
COPY ./global.json .
COPY ./EnglishWordsLearning.Application/EnglishWordsLearning.Application.csproj ./EnglishWordsLearning.Application/
COPY ./EnglishWordsLearning.Web/EnglishWordsLearning.Web.csproj ./EnglishWordsLearning.Web/
COPY ./EnglishWordsLearning.Core/EnglishWordsLearning.Core.csproj ./EnglishWordsLearning.Core/
COPY ./EnglishWordsLearning.Infrastructure/EnglishWordsLearning.Infrastructure.csproj ./EnglishWordsLearning.Infrastructure/
RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
WORKDIR /source/EnglishWordsLearning.Web
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final stage: use the runtime image and copy the published output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "EnglishWordsLearning.Web.dll", "migrate"]