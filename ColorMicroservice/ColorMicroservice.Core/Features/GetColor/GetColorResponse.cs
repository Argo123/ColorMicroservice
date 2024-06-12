using ColorMicroservice.Shared.Controllers;

namespace ColorMicroservice.Core.Features.GetColor;

public sealed record GetColorResponse(Error? Error = null) : BaseResponse(Error)
{
    public string ColorHex { get; set; }
}
