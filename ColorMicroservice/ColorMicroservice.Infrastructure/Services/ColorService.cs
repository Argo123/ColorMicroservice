using ColorMicroservice.Core.Abstractions.Services;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ColorMicroservice.Tests.Infrastructure")]
namespace ColorMicroservice.Infrastructure.Services;

internal sealed class ColorService : IColorService
{
    public string GetRandomColorHex()
    {
        var random = new Random();
        return string.Format("#{0:X6}", random.Next(0x1000000));
    }
}
