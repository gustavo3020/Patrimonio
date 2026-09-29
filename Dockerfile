FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Core
COPY src/Api/Core/Patrimonio.Api/Patrimonio.Api.csproj \
     src/Api/Core/Patrimonio.Api/

# Módulo Finanças
COPY src/Api/Financas/Patrimonio.Financas.Api/Patrimonio.Financas.Api.csproj \
     src/Api/Financas/Patrimonio.Financas.Api/

COPY src/Api/Financas/Patrimonio.Financas.Application/Patrimonio.Financas.Application.csproj \
     src/Api/Financas/Patrimonio.Financas.Application/

COPY src/Api/Financas/Patrimonio.Financas.Contracts/Patrimonio.Financas.Contracts.csproj \
     src/Api/Financas/Patrimonio.Financas.Contracts/

COPY src/Api/Financas/Patrimonio.Financas.Domain/Patrimonio.Financas.Domain.csproj \
     src/Api/Financas/Patrimonio.Financas.Domain/

COPY src/Api/Financas/Patrimonio.Financas.Infrastructure/Patrimonio.Financas.Infrastructure.csproj \
     src/Api/Financas/Patrimonio.Financas.Infrastructure/

COPY src/Api/Financas/Patrimonio.Financas.SharedKernel/Patrimonio.Financas.SharedKernel.csproj \
     src/Api/Financas/Patrimonio.Financas.SharedKernel/

RUN dotnet restore src/Api/Core/Patrimonio.Api/Patrimonio.Api.csproj

COPY src/Api/ src/Api/

RUN dotnet publish src/Api/Core/Patrimonio.Api/Patrimonio.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

# ============================================================
# Runtime
# ============================================================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "Patrimonio.Api.dll"]
