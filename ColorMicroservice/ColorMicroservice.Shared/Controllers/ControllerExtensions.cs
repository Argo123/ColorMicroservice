using Microsoft.AspNetCore.Mvc;

namespace ColorMicroservice.Shared.Controllers;

public static class ControllerExtensions
{
    public static IActionResult CreateResponse(this ControllerBase controller, BaseResponse response)
        => response.IsSuccess
            ? controller.Ok(response)
            : controller.StatusCode((int)response.Error!.StatusCode, response);
}

