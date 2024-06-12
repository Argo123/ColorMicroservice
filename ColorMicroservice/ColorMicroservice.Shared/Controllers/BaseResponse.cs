using ColorMicroservice.Shared.Exceptions;

namespace ColorMicroservice.Shared.Controllers;

public record BaseResponse(Error? Error = null)
{
    public IDictionary<string, IEnumerable<string>>? ValidationFailures { get; set; }
    public bool IsSuccess => Error is null;

    public static BaseResponse Success()
        => new();

    public static BaseResponse Failure(Exception exception)
        => new(new Error(System.Net.HttpStatusCode.InternalServerError, "Unhandled exception", exception.Message));

    public static BaseResponse Failure(BaseException exception)
        => new(new Error(exception.StatusCode, exception.Reason, exception.Message));
}
