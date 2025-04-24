namespace Core.Entity.Test;

public class UserTest
{
    public int Id { get; set; }

    // Telegram ID пользователя (или Id пользователя в вашей системе, если используете другую авторизацию)
    public long ChatId { get; set; }

    // Внешний ключ на сам тест
    public int TestId { get; set; }
    public global::Core.Entity.Test.Test Test { get; set; }

    // Индекс текущего вопроса, который пользователь должен ответить
    public int CurrentQuestionIndex { get; set; }

    // Завершил ли пользователь тест
    public bool IsCompleted { get; set; }

    // Ответы пользователя на вопросы этого теста
    public List<UserTestAnswer> Answers { get; set; } = new();
    
    // Проверяет, есть ли следующий вопрос
    public bool HasNextQuestion => CurrentQuestionIndex + 1 < Test.Questions.Count;

    // Возвращает следующий вопрос
    public TestQuestion GetNextQuestion()
    {
        if (!HasNextQuestion)
        {
            throw new InvalidOperationException("Нет следующего вопроса.");
        }

        return Test.Questions.ElementAt(CurrentQuestionIndex + 1);  // Получаем следующий вопрос
    }
}

public class UserTestAnswer
{
    public int Id { get; set; }

    // Внешний ключ на сессию прохождения теста
    public int UserTestId { get; set; }
    public UserTest UserTest { get; set; }

    // ID вопроса, на который отвечал пользователь
    public int QuestionId { get; set; }

    // Индекс выбранного варианта ответа
    public int SelectedIndex { get; set; }

    // Был ли выбранный ответ правильным
    public bool IsCorrect { get; set; }
}