# Используем официальный образ для .NET Core SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию в контейнере
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY WebAPI/WebAPI.csproj ./WebAPI/
RUN dotnet restore ./WebAPI/WebAPI.csproj

# Копируем все оставшиеся файлы приложения
COPY . ./

# Публикуем приложение в режиме релиза
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app/publish

# Используем официальный образ .NET Core runtime для финального образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Устанавливаем рабочую директорию в контейнере
WORKDIR /app

# Копируем опубликованные файлы из предыдущего образа
COPY --from=build /app/publish .

# Открываем порт, на котором будет работать приложение
EXPOSE 80

# Устанавливаем точку входа для контейнера
ENTRYPOINT ["dotnet", "WebAPI.dll"]
