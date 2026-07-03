namespace User.Application.DTOs
{
    public record TokenResponse(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpires,
        DateTime RefreshTokenExpires
    );
}
