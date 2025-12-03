using MyMoneySaver.Models;
using MyMoneySaver.Services;

namespace MyMoneySaver.Tests.Services;

/// <summary>
/// Unit tests for TransactionService
/// </summary>
public class TransactionServiceTests
{
    [Fact]
    public void GetAll_ReturnsEmptyList_OnInitialization()
    {
        // Arrange & Act
        var service = new TransactionService();

        // Assert
        var result = service.GetAll();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_ReturnsAllTransactions()
    {
        // Arrange
        var service = new TransactionService();
        var trans1 = new Transaction { Amount = 100, CategoryId = 1, Description = "Test1", Type = TransactionType.Expense };
        var trans2 = new Transaction { Amount = 200, CategoryId = 2, Description = "Test2", Type = TransactionType.Income };

        // Act
        service.Add(trans1);
        service.Add(trans2);
        var result = service.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetById_WithValidId_ReturnsTransaction()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };
        service.Add(transaction);

        // Act
        var result = service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Description);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var service = new TransactionService();

        // Act
        var result = service.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Add_CreatesNewTransaction()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };

        // Act
        service.Add(transaction);
        var result = service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(100, result.Amount);
    }

    [Fact]
    public void Add_AutoIncrementsId()
    {
        // Arrange
        var service = new TransactionService();
        var trans1 = new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Type = TransactionType.Expense };
        var trans2 = new Transaction { Amount = 200, CategoryId = 2, Description = "T2", Type = TransactionType.Income };

        // Act
        service.Add(trans1);
        service.Add(trans2);

        // Assert
        Assert.Equal(1, trans1.Id);
        Assert.Equal(2, trans2.Id);
    }

    [Fact]
    public void Add_WithNullTransaction_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new TransactionService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.Add(null!));
    }

    [Fact]
    public void Add_FiresOnTransactionsChangedEvent()
    {
        // Arrange
        var service = new TransactionService();
        var eventFired = false;
        service.OnTransactionsChanged += () => eventFired = true;
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };

        // Act
        service.Add(transaction);

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void Update_ModifiesExistingTransaction()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };
        service.Add(transaction);

        var updated = new Transaction { Id = 1, Amount = 150, CategoryId = 2, Description = "Updated", Type = TransactionType.Income };

        // Act
        service.Update(updated);
        var result = service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(150, result.Amount);
        Assert.Equal("Updated", result.Description);
        Assert.Equal(2, result.CategoryId);
    }

    [Fact]
    public void Update_WithInvalidId_DoesNotThrow()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Id = 999, Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };

        // Act & Assert - should not throw
        service.Update(transaction);
        Assert.Null(service.GetById(999));
    }

    [Fact]
    public void Update_WithNullTransaction_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new TransactionService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.Update(null!));
    }

    [Fact]
    public void Update_FiresOnTransactionsChangedEvent_WhenFound()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };
        service.Add(transaction);

        var eventFired = false;
        service.OnTransactionsChanged += () => eventFired = true;
        var updated = new Transaction { Id = 1, Amount = 200, CategoryId = 1, Description = "Updated", Type = TransactionType.Expense };

        // Act
        service.Update(updated);

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void Update_DoesNotFireEvent_WhenNotFound()
    {
        // Arrange
        var service = new TransactionService();
        var eventFired = false;
        service.OnTransactionsChanged += () => eventFired = true;
        var transaction = new Transaction { Id = 999, Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };

        // Act
        service.Update(transaction);

        // Assert
        Assert.False(eventFired);
    }

    [Fact]
    public void Delete_RemovesTransaction()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };
        service.Add(transaction);

        // Act
        service.Delete(1);
        var result = service.GetById(1);

        // Assert
        Assert.Null(result);
        Assert.Empty(service.GetAll());
    }

    [Fact]
    public void Delete_WithInvalidId_DoesNotThrow()
    {
        // Arrange
        var service = new TransactionService();

        // Act & Assert - should not throw
        service.Delete(999);
        Assert.Empty(service.GetAll());
    }

    [Fact]
    public void Delete_FiresOnTransactionsChangedEvent_WhenFound()
    {
        // Arrange
        var service = new TransactionService();
        var transaction = new Transaction { Amount = 100, CategoryId = 1, Description = "Test", Type = TransactionType.Expense };
        service.Add(transaction);

        var eventFired = false;
        service.OnTransactionsChanged += () => eventFired = true;

        // Act
        service.Delete(1);

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void Delete_DoesNotFireEvent_WhenNotFound()
    {
        // Arrange
        var service = new TransactionService();
        var eventFired = false;
        service.OnTransactionsChanged += () => eventFired = true;

        // Act
        service.Delete(999);

        // Assert
        Assert.False(eventFired);
    }

    [Fact]
    public void GetFiltered_ByCategoryId_ReturnsMatching()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 2, Description = "T2", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 150, CategoryId = 1, Description = "T3", Type = TransactionType.Income });

        // Act
        var result = service.GetFiltered(categoryId: 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(1, t.CategoryId));
    }

    [Fact]
    public void GetFiltered_ByType_ReturnsMatching()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 2, Description = "T2", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 150, CategoryId = 1, Description = "T3", Type = TransactionType.Income });

        // Act
        var result = service.GetFiltered(type: TransactionType.Income);

        // Assert
        Assert.Single(result);
        Assert.Equal(TransactionType.Income, result[0].Type);
    }

    [Fact]
    public void GetFiltered_ByStartDate_ReturnsMatching()
    {
        // Arrange
        var service = new TransactionService();
        var date1 = new DateTime(2025, 11, 01);
        var date2 = new DateTime(2025, 12, 01);
        var date3 = new DateTime(2025, 12, 15);

        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Date = date1, Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 2, Description = "T2", Date = date2, Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 150, CategoryId = 1, Description = "T3", Date = date3, Type = TransactionType.Income });

        // Act
        var result = service.GetFiltered(startDate: new DateTime(2025, 12, 01));

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.True(t.Date >= new DateTime(2025, 12, 01)));
    }

    [Fact]
    public void GetFiltered_ByEndDate_ReturnsMatching()
    {
        // Arrange
        var service = new TransactionService();
        var date1 = new DateTime(2025, 11, 01);
        var date2 = new DateTime(2025, 12, 01);
        var date3 = new DateTime(2025, 12, 15);

        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Date = date1, Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 2, Description = "T2", Date = date2, Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 150, CategoryId = 1, Description = "T3", Date = date3, Type = TransactionType.Income });

        // Act
        var result = service.GetFiltered(endDate: new DateTime(2025, 12, 10));

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.True(t.Date <= new DateTime(2025, 12, 10)));
    }

    [Fact]
    public void GetFiltered_WithCombinedFilters_ReturnsMatching()
    {
        // Arrange
        var service = new TransactionService();
        var date1 = new DateTime(2025, 12, 01);
        var date2 = new DateTime(2025, 12, 05);
        var date3 = new DateTime(2025, 12, 15);

        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Date = date1, Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 1, Description = "T2", Date = date2, Type = TransactionType.Income });
        service.Add(new Transaction { Amount = 150, CategoryId = 2, Description = "T3", Date = date3, Type = TransactionType.Income });

        // Act
        var result = service.GetFiltered(
            categoryId: 1,
            startDate: new DateTime(2025, 12, 01),
            endDate: new DateTime(2025, 12, 10),
            type: TransactionType.Income);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].CategoryId);
        Assert.Equal(TransactionType.Income, result[0].Type);
        Assert.Equal(date2, result[0].Date);
    }

    [Fact]
    public void GetFiltered_WithNoFilters_ReturnsAll()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 2, Description = "T2", Type = TransactionType.Income });
        service.Add(new Transaction { Amount = 150, CategoryId = 1, Description = "T3", Type = TransactionType.Expense });

        // Act
        var result = service.GetFiltered();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetTotalBalance_CalculatesIncomePlusExpenses()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 1000, CategoryId = 1, Description = "Income", Type = TransactionType.Income });
        service.Add(new Transaction { Amount = 300, CategoryId = 1, Description = "Expense1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 1, Description = "Expense2", Type = TransactionType.Expense });

        // Act
        var result = service.GetTotalBalance();

        // Assert
        Assert.Equal(500, result); // 1000 - 300 - 200
    }

    [Fact]
    public void GetTotalBalance_EmptyTransactions_ReturnsZero()
    {
        // Arrange
        var service = new TransactionService();

        // Act
        var result = service.GetTotalBalance();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalIncome_SumsIncomeTransactions()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 500, CategoryId = 1, Description = "Income1", Type = TransactionType.Income });
        service.Add(new Transaction { Amount = 300, CategoryId = 1, Description = "Expense", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 700, CategoryId = 1, Description = "Income2", Type = TransactionType.Income });

        // Act
        var result = service.GetTotalIncome();

        // Assert
        Assert.Equal(1200, result);
    }

    [Fact]
    public void GetTotalIncome_NoIncomeTransactions_ReturnsZero()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "Expense1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 1, Description = "Expense2", Type = TransactionType.Expense });

        // Act
        var result = service.GetTotalIncome();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetTotalExpenses_SumsExpenseTransactions()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 500, CategoryId = 1, Description = "Income", Type = TransactionType.Income });
        service.Add(new Transaction { Amount = 300, CategoryId = 1, Description = "Expense1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 1, Description = "Expense2", Type = TransactionType.Expense });

        // Act
        var result = service.GetTotalExpenses();

        // Assert
        Assert.Equal(500, result);
    }

    [Fact]
    public void GetTotalExpenses_NoExpenseTransactions_ReturnsZero()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "Income1", Type = TransactionType.Income });
        service.Add(new Transaction { Amount = 200, CategoryId = 1, Description = "Income2", Type = TransactionType.Income });

        // Act
        var result = service.GetTotalExpenses();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCategoryTotals_GroupsAndSumsByCategory()
    {
        // Arrange
        var service = new TransactionService();
        service.Add(new Transaction { Amount = 100, CategoryId = 1, Description = "T1", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 200, CategoryId = 1, Description = "T2", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 150, CategoryId = 2, Description = "T3", Type = TransactionType.Expense });
        service.Add(new Transaction { Amount = 50, CategoryId = 2, Description = "T4", Type = TransactionType.Expense });

        // Act
        var result = service.GetCategoryTotals();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(300, result[1]); // 100 + 200
        Assert.Equal(200, result[2]); // 150 + 50
    }

    [Fact]
    public void GetCategoryTotals_EmptyTransactions_ReturnsEmpty()
    {
        // Arrange
        var service = new TransactionService();

        // Act
        var result = service.GetCategoryTotals();

        // Assert
        Assert.Empty(result);
    }
}
