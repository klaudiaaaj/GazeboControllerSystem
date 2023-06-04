FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KaffkaSubscriber/KaffkaSubscriber.csproj", "KaffkaSubscriber/"]
RUN dotnet restore "KaffkaSubscriber/KaffkaSubscriber.csproj"
COPY . .
WORKDIR "/src/KaffkaSubscriber"
RUN dotnet build "KaffkaSubscriber.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KaffkaSubscriber.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KaffkaSubscriber.dll"]