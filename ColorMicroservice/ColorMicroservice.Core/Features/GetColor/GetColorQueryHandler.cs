using ColorMicroservice.Core.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ColorMicroservice.Core.Features.GetColor;

public sealed class GetColorQueryHandler : IRequestHandler<GetColorQuery, GetColorResponse>
{
    private readonly ILogger<GetColorQueryHandler> _logger;
    private readonly IColorService _colorService;

    public GetColorQueryHandler(ILogger<GetColorQueryHandler> logger, IColorService colorService)
    {
        _logger = logger;
        _colorService = colorService;
    }

    public async Task<GetColorResponse> Handle(GetColorQuery request, CancellationToken cancellationToken)
    {
        return new GetColorResponse
        {
            ColorHex = _colorService.GetRandomColorHex(),
        };
    }
}
