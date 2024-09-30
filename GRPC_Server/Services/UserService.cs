using Grpc.Core;

namespace GRPC_Server.Services;

public class UserService : User.UserBase
{
    public override Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
    {
        // Basit bir örnek yanıt döndürüyoruz.
        var user = new UserResponse
        {
            UserId = request.UserId,
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        return Task.FromResult(user);
    }
}
