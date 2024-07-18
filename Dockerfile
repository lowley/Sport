# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 80
EXPOSE 443

# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build2
COPY "/Tools" "/src/Tools"
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/PWA4"
COPY ["PWA4/PWA4.csproj", "."]
RUN dotnet restore "PWA4.csproj"
COPY PWA3 .
RUN dotnet build "PWA4.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM build2 AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/PWA4"
RUN dotnet publish "PWA4.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD dotnet PWA4.dll
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet PWA4.dll
