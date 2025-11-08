# 1. Сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ИСПРАВЛЕНЫ ПУТИ:
# Копируем .csproj всех проектов, от которых зависит Api
# (Предполагается, что остальные папки - Application, Domain - лежат рядом)
COPY ["OnlineQuizzApi/OnlineQuizzApi.csproj", "OnlineQuizzApi/"]
COPY ["QuizManager.Application/QuizManager.Application.csproj", "QuizManager.Application/"]
COPY ["QuizManager.Domain/QuizManager.Domain.csproj", "QuizManager.Domain/"]
COPY ["QuizManager.Infrastructure/QuizManager.Infrastructure.csproj", "QuizManager.Infrastructure/"]

RUN dotnet restore "OnlineQuizzApi/OnlineQuizzApi.csproj"
COPY . .
WORKDIR "/src/OnlineQuizzApi" 
RUN dotnet build "OnlineQuizzApi.csproj" -c Release -o /app/build

# 2. Публикация
FROM build AS publish
RUN dotnet publish "OnlineQuizzApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineQuizzApi.dll"] # <-- ИСПРАВЛЕНО