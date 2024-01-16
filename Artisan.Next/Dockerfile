FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Artisan.Next/Artisan.Next.csproj", "Artisan.Next/"]
COPY ["Artisan.Next.Client/Artisan.Next.Client.csproj", "Artisan.Next.Client/"]
RUN dotnet restore "Artisan.Next/Artisan.Next.csproj"
COPY . .
WORKDIR "/src/Artisan.Next"
RUN dotnet build "Artisan.Next.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Artisan.Next.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artisan.Next.dll"]
