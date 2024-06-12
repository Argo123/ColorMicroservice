using ColorMicroservice.Core.Features.GetColor;
using ColorMicroservice.Shared.Controllers;
using ColorMicroservice.Shared.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ColorMicroservice.API.Controllers;

[Route("api/color")]
public sealed class ColorController : BaseApiController
{
    public ColorController(IControllerRequestHandler controllerRequestHandler) : base(controllerRequestHandler)
    { }

    [HttpGet("getcolor")]
    public async Task<IActionResult> GetColor()
        => await ControllerRequestHandler.HandleRequestAsync<GetColorQuery, GetColorResponse>(
            this,
            new GetColorQuery(),
            HttpContext.RequestAborted);
}
