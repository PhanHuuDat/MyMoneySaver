using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using MyMoneySaver.Models;
using Xunit;

namespace MyMoneySaver.Tests.Models;

public class TransactionTests
{
    [Fact]
    public void Transaction_WithValidData_ShouldBeValid()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1,
            Amount = 50.00m,
            CategoryId = 1,
            Description = "Lunch at restaurant",
            Date = new DateTime(2025, 12, 2),
            Type = TransactionType.Expense
        };

        // Act
        var validationResults = ValidateModel(transaction);

        // Assert
        validationResults.Should().BeEmpty();
        transaction.Amount.Should().Be(50.00m);
        transaction.CategoryId.Should().Be(1);
        transaction.Description.Should().Be("Lunch at restaurant");
    }

    [Theory]
    [InlineData(0.01, true)]
    [InlineData(1.00, true)]
    [InlineData(100.50, true)]
    [InlineData(1000000, true)]
    [InlineData(0.00, false)]
    [InlineData(-1.00, false)]
    [InlineData(1000000.01, false)]
    public void Transaction_AmountValidation(decimal amount, bool isValid)
    {
        // Arrange
        var transaction = new Transaction
        {
            Amount = amount,
            CategoryId = 1,
            Description = "Test transaction",
            Date = DateTime.Today,
            Type = TransactionType.Expense
        };

        // Act
        var validationResults = ValidateModel(transaction);

        // Assert
        if (isValid)
        {
            validationResults.Should().BeEmpty();
        }
        else
        {
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("Amount"));
        }
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void Transaction_CategoryIdValidation(int categoryId, bool isValid)
    {
        // Arrange
        var transaction = new Transaction
        {
            Amount = 50.00m,
            CategoryId = categoryId,
            Description = "Test transaction",
            Date = DateTime.Today,
            Type = TransactionType.Expense
        };

        // Act
        var validationResults = ValidateModel(transaction);

        // Assert
        if (isValid)
        {
            validationResults.Should().BeEmpty();
        }
        else
        {
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("CategoryId"));
        }
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("A", true)]
    [InlineData("Valid description", true)]
    public void Transaction_DescriptionValidation(string description, bool isValid)
    {
        // Arrange
        var transaction = new Transaction
        {
            Amount = 50.00m,
            CategoryId = 1,
            Description = description,
            Date = DateTime.Today,
            Type = TransactionType.Expense
        };

        // Act
        var validationResults = ValidateModel(transaction);

        // Assert
        if (isValid)
        {
            validationResults.Should().BeEmpty();
        }
        else
        {
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("Description"));
        }
    }

    [Fact]
    public void Transaction_DescriptionMaxLength_Validation()
    {
        // Arrange
        var longDescription = new string('A', 201);
        var transaction = new Transaction
        {
            Amount = 50.00m,
            CategoryId = 1,
            Description = longDescription,
            Date = DateTime.Today,
            Type = TransactionType.Expense
        };

        // Act
        var validationResults = ValidateModel(transaction);

        // Assert
        validationResults.Should().ContainSingle();
        validationResults.First().MemberNames.Should().Contain("Description");
    }

    [Fact]
    public void Transaction_DefaultValues_ShouldBeSet()
    {
        // Act
        var transaction = new Transaction();

        // Assert
        transaction.Description.Should().Be(string.Empty);
        transaction.Date.Should().Be(DateTime.Today);
        transaction.Type.Should().Be(TransactionType.Expense);
    }

    [Theory]
    [InlineData(TransactionType.Expense)]
    [InlineData(TransactionType.Income)]
    public void Transaction_TypeValidation(TransactionType type)
    {
        // Arrange
        var transaction = new Transaction
        {
            Amount = 50.00m,
            CategoryId = 1,
            Description = "Test",
            Date = DateTime.Today,
            Type = type
        };

        // Act
        var validationResults = ValidateModel(transaction);

        // Assert
        validationResults.Should().BeEmpty();
        transaction.Type.Should().Be(type);
    }

    [Fact]
    public void Transaction_CategoryNavigationProperty_CanBeNull()
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Amount = 50.00m,
            CategoryId = 1,
            Description = "Test",
            Category = null
        };

        // Assert
        transaction.Category.Should().BeNull();
    }

    [Fact]
    public void Transaction_CategoryNavigationProperty_CanBeSet()
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
        var transaction = new Transaction
        {
            Amount = 50.00m,
            CategoryId = 1,
            Description = "Test",
            Category = category
        };

        // Assert
        transaction.Category.Should().NotBeNull();
        transaction.Category.Name.Should().Be("Food");
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}
