using ColorMicroservice.Core.Abstractions.Services;
using ColorMicroservice.Infrastructure.Services;
using System.Text.RegularExpressions;

namespace ColorMicroservice.Tests.Infrastructure;

[TestClass]
public partial class ColorServiceTests
{
    [GeneratedRegex(@"^(?:[0-9a-fA-F]{6})$")]
    private static partial Regex ValidCharactersRegex();

    private readonly IColorService _colorService;

    public ColorServiceTests()
    {
        _colorService = new ColorService();
    }

    [TestMethod]
    public void Test_ColorLength()
    {
        var color = _colorService.GetRandomColorHex();
        Assert.AreEqual(7, color?.Length, message: "Invalid hex color length");
    }

    [TestMethod]
    public void Test_ColorPrefix()
    {
        var color = _colorService.GetRandomColorHex();
        Assert.AreEqual('#', color[0], message: "Invalid hex prefix");
    }

    [TestMethod]
    public void Test_ColorValidCharacters()
    {
        var color = _colorService.GetRandomColorHex();
        Assert.IsTrue(ValidCharactersRegex().IsMatch(color[1..]), message: "Invalid hexadecimal characters");
    }
}
