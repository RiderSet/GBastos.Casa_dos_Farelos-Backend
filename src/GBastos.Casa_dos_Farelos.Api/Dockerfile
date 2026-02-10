# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GBastos.Casa_dos_Farelos.Api/GBastos.Casa_dos_Farelos.Api.csproj", "src/GBastos.Casa_dos_Farelos.Api/"]
COPY ["src/GBastos.Casa_dos_Farelos.Application/GBastos.Casa_dos_Farelos.Application.csproj", "src/GBastos.Casa_dos_Farelos.Application/"]
COPY ["src/GBastos.Casa_dos_Farelos.Domain/GBastos.Casa_dos_Farelos.Domain.csproj", "src/GBastos.Casa_dos_Farelos.Domain/"]
COPY ["src/GBastos.Casa_dos_Farelos.Infrastructure/GBastos.Casa_dos_Farelos.Infrastructure.csproj", "src/GBastos.Casa_dos_Farelos.Infrastructure/"]
COPY ["src/GBastos.Casa_dos_Farelos.Shared/GBastos.Casa_dos_Farelos.Shared.csproj", "src/GBastos.Casa_dos_Farelos.Shared/"]

RUN dotnet restore "./src/GBastos.Casa_dos_Farelos.Api/GBastos.Casa_dos_Farelos.Api.csproj"
COPY . .
WORKDIR "/src/src/GBastos.Casa_dos_Farelos.Api"
RUN dotnet build "./GBastos.Casa_dos_Farelos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GBastos.Casa_dos_Farelos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/GBastos.Casa_dos_Farelos.Api/appsettings.json .
ENTRYPOINT ["dotnet", "GBastos.Casa_dos_Farelos.Api.dll"]