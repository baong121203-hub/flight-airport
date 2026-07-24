using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Model;

namespace FlightApi.Dto;

public static class UserMapper
{
    public static User ToEntity(CreateUserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = request.Password,
            FullName = request.FullName,
            Email = request.Email
        };
    }

    public static void ApplyUpdate(User user, UpdateUserRequest request)
    {
        user.Username = request.Username;
        user.Password = request.Password;
        user.FullName = request.FullName;
        user.Email = request.Email;
    }

    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email
        };
    }

    public static List<UserResponse> ToResponseList(IEnumerable<User> users)
    {
        return users.Select(ToResponse).ToList();
    }
}
