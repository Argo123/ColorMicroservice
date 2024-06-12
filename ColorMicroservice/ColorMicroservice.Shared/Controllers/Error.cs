using System.Net;

namespace ColorMicroservice.Shared.Controllers;

public record Error
(
    HttpStatusCode StatusCode,
    string Reason,
    string Message
);
