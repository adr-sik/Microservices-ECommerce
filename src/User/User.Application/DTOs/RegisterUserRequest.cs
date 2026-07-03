namespace User.Application.DTOs
{
    public record RegisterUserRequest(
        string Name,
        string Email,
        string Password
    );
}
