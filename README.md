# BarsCourseTelegramBot

## Описание

Проект, реализованный в рамках Хакатона от Bars Group. 
Телеграм бот для доступа к обучающему курсу, админ панель, лонгрид сайт с материалами для обучения

## Как развернуть? 

1. Клонируем репозиторий 

2. Создаем в папке Bot и WebAPI по примеру .env.example вместо этого файла:
   - файл .env.docker если планируешь поднимать в Docker
   - файл .env.local если запускаешь через ide

3. Какие переменные окружения вписывать?

Это телеграм токен который можно получить через @BotFather бота в телеграме, создав бота
BotConfiguration__Token=...

Это telegram id для админа - можно получить в боте @getmyid_bot  
BotConfiguration__AdminChatId=...

Для этой строки подключения к базе данных есть два пути
ConnectionStrings__DefaultConnection="Host=...;Database=...;Username=...;Password=...;port=..."

**Если хочешь поднять все в docker и создал .env.docker:**
   - host - имя контейнера бд
   - database - возьми из docker-compose yml в данных контейнера бд
   - username - возьми из docker-compose yml в данных контейнера бд
   - password - возьми из docker-compose yml в данных контейнера бд
   - port - - возьми из docker-compose yml в данных контейнера бд(обычно 5432 всегда)

**Если хочешь запустить в ide и создал .env.local:**
   - host - localhost
   - database - название твоей бд (можно указать любое и не создавать бд даже в целом)
   - username - ник пользователя твоей postgres бд(требует установленной postgres)
   - password - пароль пользователя твоей postgres бд(требует установленной postgres)
   - port - порт твоей postgres бд(обычно 5432)

4.(Для docker) Открываем в терминале корневую папку проекта и вводим команду:
   - docker-compose up

Готово!

P.s. фронт в папках frontend и longread, но писать о том как его поднять я не буду
и в докер композ его тож нету :(