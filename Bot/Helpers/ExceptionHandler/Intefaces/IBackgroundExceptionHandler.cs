namespace Bot.Helpers.ExceptionHandler.Intefaces;

public interface IBackgroundExceptionHandler
{
    public Task HandleExceptionAsync(Exception ex);
}