using MyMoneySaver.Models;
using MyMoneySaver.Services;

namespace MyMoneySaver.Tests.Services;

/// <summary>
/// Unit tests for CategoryService
/// </summary>
public class CategoryServiceTests
{
    [Fact]
    public void Constructor_SeedsDefaultCategories()
    {
        // Arrange & Act
        var service = new CategoryService();

        // Assert
        var categories = service.GetAll();
        Assert.NotNull(categories);
        Assert.Equal(6, categories.Count);
    }

    [Fact]
    public void Constructor_AssignsSequentialIds()
    {
        // Arrange & Act
        var service = new CategoryService();
        var categories = service.GetAll();

        // Assert
        var expectedIds = new[] { 1, 2, 3, 4, 5, 6 };
        var actualIds = categories.Select(c => c.Id).ToList();
        Assert.Equal(expectedIds, actualIds);
    }

    [Theory]
    [InlineData(1, "Food", "restaurant", "#ff9800")]
    [InlineData(2, "Transport", "directions_car", "#2196f3")]
    [InlineData(3, "Entertainment", "movie", "#e91e63")]
    [InlineData(4, "Shopping", "shopping_cart", "#9c27b0")]
    [InlineData(5, "Bills", "receipt", "#f44336")]
    [InlineData(6, "Other", "category", "#607d8b")]
    public void Constructor_SeedsCorrectCategoryProperties(int id, string name, string icon, string color)
    {
        // Arrange & Act
        var service = new CategoryService();
        var category = service.GetById(id);

        // Assert
        Assert.NotNull(category);
        Assert.Equal(name, category.Name);
        Assert.Equal(icon, category.Icon);
        Assert.Equal(color, category.Color);
    }

    [Fact]
    public void GetAll_ReturnsAllCategories()
    {
        // Arrange
        var service = new CategoryService();

        // Act
        var result = service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Count);
    }

    [Fact]
    public void GetById_WithValidId_ReturnsCategory()
    {
        // Arrange
        var service = new CategoryService();

        // Act
        var result = service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Food", result.Name);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var service = new CategoryService();

        // Act
        var result = service.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Add_CreatesNewCategory()
    {
        // Arrange
        var service = new CategoryService();
        var newCategory = new Category { Name = "Health", Icon = "health", Color = "#4caf50" };

        // Act
        service.Add(newCategory);
        var result = service.GetById(7);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Health", result.Name);
        Assert.Equal(7, result.Id);
    }

    [Fact]
    public void Add_AutoIncrementsId()
    {
        // Arrange
        var service = new CategoryService();
        var cat1 = new Category { Name = "Cat1", Icon = "icon1", Color = "#111111" };
        var cat2 = new Category { Name = "Cat2", Icon = "icon2", Color = "#222222" };

        // Act
        service.Add(cat1);
        service.Add(cat2);

        // Assert
        Assert.Equal(7, cat1.Id);
        Assert.Equal(8, cat2.Id);
    }

    [Fact]
    public void Add_WithNullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new CategoryService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.Add(null!));
    }

    [Fact]
    public void Add_FiresOnCategoriesChangedEvent()
    {
        // Arrange
        var service = new CategoryService();
        var eventFired = false;
        service.OnCategoriesChanged += () => eventFired = true;
        var newCategory = new Category { Name = "Test", Icon = "test", Color = "#ffffff" };

        // Act
        service.Add(newCategory);

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void Update_ModifiesExistingCategory()
    {
        // Arrange
        var service = new CategoryService();
        var categoryToUpdate = new Category { Id = 1, Name = "Updated Food", Icon = "food_updated", Color = "#cccccc" };

        // Act
        service.Update(categoryToUpdate);
        var result = service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Food", result.Name);
        Assert.Equal("food_updated", result.Icon);
    }

    [Fact]
    public void Update_WithInvalidId_DoesNotThrow()
    {
        // Arrange
        var service = new CategoryService();
        var categoryToUpdate = new Category { Id = 999, Name = "NonExistent", Icon = "icon", Color = "#aaaaaa" };

        // Act & Assert - should not throw
        service.Update(categoryToUpdate);
        Assert.Null(service.GetById(999));
    }

    [Fact]
    public void Update_WithNullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new CategoryService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.Update(null!));
    }

    [Fact]
    public void Update_FiresOnCategoriesChangedEvent_WhenFound()
    {
        // Arrange
        var service = new CategoryService();
        var eventFired = false;
        service.OnCategoriesChanged += () => eventFired = true;
        var categoryToUpdate = new Category { Id = 1, Name = "Updated", Icon = "upd", Color = "#bbbbbb" };

        // Act
        service.Update(categoryToUpdate);

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void Update_DoesNotFireEvent_WhenNotFound()
    {
        // Arrange
        var service = new CategoryService();
        var eventFired = false;
        service.OnCategoriesChanged += () => eventFired = true;
        var categoryToUpdate = new Category { Id = 999, Name = "NonExistent", Icon = "icon", Color = "#dddddd" };

        // Act
        service.Update(categoryToUpdate);

        // Assert
        Assert.False(eventFired);
    }

    [Fact]
    public void Delete_RemovesCategory()
    {
        // Arrange
        var service = new CategoryService();

        // Act
        service.Delete(1);
        var result = service.GetById(1);

        // Assert
        Assert.Null(result);
        Assert.Equal(5, service.GetAll().Count);
    }

    [Fact]
    public void Delete_WithInvalidId_DoesNotThrow()
    {
        // Arrange
        var service = new CategoryService();

        // Act & Assert - should not throw
        service.Delete(999);
        Assert.Equal(6, service.GetAll().Count);
    }

    [Fact]
    public void Delete_FiresOnCategoriesChangedEvent_WhenFound()
    {
        // Arrange
        var service = new CategoryService();
        var eventFired = false;
        service.OnCategoriesChanged += () => eventFired = true;

        // Act
        service.Delete(1);

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void Delete_DoesNotFireEvent_WhenNotFound()
    {
        // Arrange
        var service = new CategoryService();
        var eventFired = false;
        service.OnCategoriesChanged += () => eventFired = true;

        // Act
        service.Delete(999);

        // Assert
        Assert.False(eventFired);
    }
}
