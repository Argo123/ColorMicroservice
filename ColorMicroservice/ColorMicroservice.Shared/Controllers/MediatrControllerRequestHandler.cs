using ColorMicroservice.Shared.Controllers.Interfaces;
using ColorMicroservice.Shared.Serialization;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ColorMicroservice.Shared.Controllers;

internal sealed class MediatrControllerRequestHandler : IControllerRequestHandler
{
    private readonly ILogger<MediatrControllerRequestHandler> _logger;
    private readonly IMediator _mediator;

    public MediatrControllerRequestHandler(
        ILogger<MediatrControllerRequestHandler> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> HandleRequestAsync<TRequest, TResponse>(
        ControllerBase controller,
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>, IBaseRequest
        where TResponse : BaseResponse
    {
        _logger.LogInformation("Sending request {Request}", request.GetType().Name);

        var validationFailures = await ValidateRequestAsync(controller, request, cancellationToken);

        if (validationFailures.Any())
        {
            var validationFailureResponse =
                new BaseResponse(new Error(
                    HttpStatusCode.BadRequest,
                    "ValidationFailed",
                    "One or more validation failures have occurred"))
                { ValidationFailures = validationFailures };

            _logger.LogWarning("Request validation failed. Validation failures:\n{ValidationFailures}",
                validationFailures.ToJson(JsonSettings.DefaultIndented));

            return controller.CreateResponse(validationFailureResponse);
        }

        var response = await _mediator.Send(request, cancellationToken);
        _logger.LogInformation("Request {Request} completed", request.GetType().Name);

        return controller.CreateResponse(response);
    }

    public async Task<IActionResult> HandleRequestAsync<TRequest>(
        ControllerBase controller,
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        _logger.LogInformation("Sending request {Request}", request.GetType().Name);

        var validationFailures = await ValidateRequestAsync(controller, request, cancellationToken);

        if (validationFailures.Any())
        {
            var validationFailureResponse =
                new BaseResponse(new Error(
                    HttpStatusCode.BadRequest,
                    "ValidationFailed",
                    "One or more validation failures have occurred"))
                { ValidationFailures = validationFailures };

            _logger.LogWarning("Request validation failed. Validation failures:\n{ValidationFailures}",
                validationFailures.ToJson(JsonSettings.DefaultIndented));

            return controller.CreateResponse(validationFailureResponse);
        }

        await _mediator.Send(request, cancellationToken);
        _logger.LogInformation("Request {Request} completed", request.GetType().Name);

        return controller.Ok();
    }

    private async Task<IDictionary<string, IEnumerable<string>>> ValidateRequestAsync<TRequest>(
        ControllerBase controller,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : IBaseRequest
    {
        var services = controller.HttpContext.RequestServices;
        var validator = services.GetService<IValidator<TRequest>>();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationFailures = ToValidationFailures(validationResult.Errors);

                return validationFailures;
            }
        }

        return new Dictionary<string, IEnumerable<string>>();
    }

    private IDictionary<string, IEnumerable<string>> ToValidationFailures(IEnumerable<ValidationFailure> failures)
        => failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(group => group.Key, group => group.AsEnumerable());
}

