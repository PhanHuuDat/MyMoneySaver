using FluentAssertions;

namespace MyMoneySaver.Tests;

public class SampleTests
{
    [Fact]
    public void Sample_Test_Passes()
    {
        // Arrange
        var value = 42;

        // Act & Assert
        value.Should().Be(42);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(5, 5, 10)]
    public void Sample_Theory_Test(int a, int b, int expected)
    {
        // Act
        var result = a + b;

        // Assert
        result.Should().Be(expected);
    }
}
