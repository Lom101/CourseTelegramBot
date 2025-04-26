using Backend.Dto.User.Request;
using Backend.Dto.User.Response;
using Core.Dto.Request;
using Core.Entity;

namespace Backend.Mapper;

public static class UserMapper
{
    public static GetUserResponse ToDto(User user)
    {
        return new GetUserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            ChatId = user.ChatId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RegistrationDate = user.RegistrationDate,
            LastActivity = user.LastActivity,
            IsBlocked = user.IsBlocked,
            IsAdmin = user.IsAdmin
        };
    }

    public static User ToEntity(CreateUserRequest request)
    {
        return new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsAdmin = false,
            IsBlocked = false,
            RegistrationDate = DateTime.UtcNow
        };
    }
    
    public static User ToEntity(User user, UpdateUserRequest request)
    {
        user.FullName = request.FullName;
        user.Email = request.Email;
        if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
        user.ChatId = request.ChatId;
        return user;
    }
}
