using Xunit;

namespace Starter.Tests;

public class StringCalculatorTests
{
    [Fact]
    public void Empty_text_returns_zero()
    {
        var result = StringCalculator.Add("");
        Assert.Equal(0, result);
    }
}