namespace Shared.Application.Auth;

public record AuthResult<T>(bool IsSuccess, T? Value, string? ErrorCode, string? ErrorMessage)
{
    public static AuthResult<T> Ok(T value) => new(true, value, null, null);
    public static AuthResult<T> Fail(string code, string message) => new(false, default, code, message);
}
