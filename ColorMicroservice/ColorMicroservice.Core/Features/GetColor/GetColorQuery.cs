using MediatR;

namespace ColorMicroservice.Core.Features.GetColor;

public sealed class GetColorQuery : IRequest<GetColorResponse>
{ }
