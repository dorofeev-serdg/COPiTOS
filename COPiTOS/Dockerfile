FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["/", ""]
RUN dotnet restore "COPiTOS.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "COPiTOS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "COPiTOS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Production
RUN echo "ASPNETCORE_ENVIRONMENT = $ASPNETCORE_ENVIRONMENT"
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "COPiTOS.dll"]
