using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ColorMicroservice.Shared.Controllers.Interfaces;

public interface IControllerRequestHandler
{
    Task<IActionResult> HandleRequestAsync<TRequest, TResponse>(
        ControllerBase controller,
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>, IBaseRequest
        where TResponse : BaseResponse;

    Task<IActionResult> HandleRequestAsync<TRequest>(ControllerBase controller, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}

