FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-stage
WORKDIR /source

COPY CineMatrix/*.csproj ./CineMatrix/
COPY CineMatrix.Domain/*.csproj ./CineMatrix.Domain/
COPY CineMatrix.Repository/*.csproj ./CineMatrix.Repository/
COPY CineMatrix.Service/*.csproj ./CineMatrix.Service/

RUN dotnet restore ./CineMatrix/CineMatrix.Web.csproj

COPY CineMatrix/. ./CineMatrix/
COPY CineMatrix.Domain/. ./CineMatrix.Domain/
COPY CineMatrix.Repository/. ./CineMatrix.Repository/
COPY CineMatrix.Service/. ./CineMatrix.Service/

RUN dotnet publish ./CineMatrix/CineMatrix.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-stage
WORKDIR /app

COPY --from=build-stage /app/publish .

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app
USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "CineMatrix.Web.dll"]