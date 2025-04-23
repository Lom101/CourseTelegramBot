namespace Core.Model;

public class UserFilterModel
{
    public string FullName { get; set; }  // ФИО
    public int? CompletedMaterialCount { get; set; }  // Количество пройденного материала
    public DateTime? RegistrationDateFrom { get; set; }  // Начальная дата регистрации
    public DateTime? RegistrationDateTo { get; set; }  // Конечная дата регистрации
    public bool? IsBlocked { get; set; }  // Статус блокировки
    public bool? IsAdmin { get; set; }  // Статус администратора
}
