using ColorMicroservice.Shared.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ColorMicroservice.Shared.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected readonly IControllerRequestHandler ControllerRequestHandler;

    protected BaseApiController(IControllerRequestHandler controllerRequestHandler)
    {
        ControllerRequestHandler = controllerRequestHandler;
    }
}
