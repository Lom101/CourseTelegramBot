# Используем официальный образ для .NET Core SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию в контейнере
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY Bot/Bot.csproj ./Bot/
RUN dotnet restore ./Bot/Bot.csproj

# Копируем все оставшиеся файлы приложения
COPY . ./

# Публикуем приложение в режиме релиза
RUN dotnet publish Bot/Bot.csproj -c Release -o /app/publish

# Используем официальный образ .NET Core runtime для финального образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Устанавливаем рабочую директорию в контейнере
WORKDIR /app

# Копируем опубликованные файлы из предыдущего образа
COPY --from=build /app/publish .

# Точка входа для запуска бота
ENTRYPOINT ["dotnet", "Bot.dll"]
