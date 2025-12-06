using FluentAssertions;
using MyMoneySaver.Models;
using System.ComponentModel.DataAnnotations;

namespace MyMoneySaver.Tests.Models;

public class CategoryTests
{
    [Fact]
    public void Category_WithValidData_ShouldBeValid()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Food",
            Icon = "restaurant",
            Color = "#ff9800"
        };

        // Act
        var validationResults = ValidateModel(category);

        // Assert
        validationResults.Should().BeEmpty();
        category.Name.Should().Be("Food");
        category.Icon.Should().Be("restaurant");
        category.Color.Should().Be("#ff9800");
    }

    [Theory]
    [InlineData("", false, "Name")]
    [InlineData(null, false, "Name")]
    [InlineData("A", true, null)] // MinimumLength = 1
    [InlineData("Valid Category Name", true, null)]
    public void Category_NameValidation(string name, bool isValid, string? expectedErrorField)
    {
        // Arrange
        var category = new Category
        {
            Name = name,
            Icon = "category",
            Color = "#1976d2"
        };

        // Act
        var validationResults = ValidateModel(category);

        // Assert
        if (isValid)
        {
            validationResults.Should().BeEmpty();
        }
        else
        {
            validationResults.Should().ContainSingle();
            validationResults.First().MemberNames.Should().Contain(expectedErrorField);
        }
    }

    [Theory]
    [InlineData("#ff9800", true)]
    [InlineData("#FF9800", true)]
    [InlineData("#1976d2", true)]
    [InlineData("#000000", true)]
    [InlineData("#FFFFFF", true)]
    [InlineData("#ffffff", true)]
    [InlineData("ff9800", false)]   // Missing #
    [InlineData("#ff980", false)]   // Too short
    [InlineData("#ff98000", false)] // Too long
    [InlineData("#gggggg", false)]  // Invalid hex
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Category_ColorValidation(string color, bool isValid)
    {
        // Arrange
        var category = new Category
        {
            Name = "Test",
            Icon = "test",
            Color = color
        };

        // Act
        var validationResults = ValidateModel(category);

        // Assert
        if (isValid)
        {
            validationResults.Should().BeEmpty();
        }
        else
        {
            validationResults.Should().NotBeEmpty();
            var colorError = validationResults.FirstOrDefault(v => v.MemberNames.Contains("Color"));
            colorError.Should().NotBeNull();
        }
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("restaurant", true)]
    [InlineData("directions_car", true)]
    public void Category_IconValidation(string icon, bool isValid)
    {
        // Arrange
        var category = new Category
        {
            Name = "Test",
            Icon = icon,
            Color = "#1976d2"
        };

        // Act
        var validationResults = ValidateModel(category);

        // Assert
        if (isValid)
        {
            validationResults.Should().BeEmpty();
        }
        else
        {
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("Icon"));
        }
    }

    [Fact]
    public void Category_DefaultValues_ShouldBeSet()
    {
        // Act
        var category = new Category();

        // Assert
        category.Name.Should().Be(string.Empty);
        category.Icon.Should().Be("category");
        category.Color.Should().Be("#1976d2");
    }

    [Fact]
    public void Category_MaxLength_Validation()
    {
        // Arrange
        var longName = new string('A', 51);
        var category = new Category
        {
            Name = longName,
            Icon = "test",
            Color = "#1976d2"
        };

        // Act
        var validationResults = ValidateModel(category);

        // Assert
        validationResults.Should().ContainSingle();
        validationResults.First().MemberNames.Should().Contain("Name");
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}
