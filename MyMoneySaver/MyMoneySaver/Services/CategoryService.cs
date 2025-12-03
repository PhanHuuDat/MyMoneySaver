using MyMoneySaver.Models;

namespace MyMoneySaver.Services;

/// <summary>
/// Manages categories with in-memory storage
/// </summary>
public class CategoryService
{
    private readonly List<Category> _categories = new();
    private int _nextId = 1;

    /// <summary>
    /// Event fired when categories change
    /// </summary>
    public event Action? OnCategoriesChanged;

    /// <summary>
    /// Initializes service with default categories
    /// </summary>
    public CategoryService()
    {
        SeedDefaultCategories();
    }

    /// <summary>
    /// Seeds default category data
    /// </summary>
    private void SeedDefaultCategories()
    {
        _categories.AddRange(new[]
        {
            new Category
            {
                Id = _nextId++,
                Name = "Food",
                Icon = "restaurant",
                Color = "#ff9800"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Transport",
                Icon = "directions_car",
                Color = "#2196f3"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Entertainment",
                Icon = "movie",
                Color = "#e91e63"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Shopping",
                Icon = "shopping_cart",
                Color = "#9c27b0"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Bills",
                Icon = "receipt",
                Color = "#f44336"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Other",
                Icon = "category",
                Color = "#607d8b"
            }
        });
    }

    /// <summary>
    /// Gets all categories
    /// </summary>
    public List<Category> GetAll() => _categories;

    /// <summary>
    /// Gets category by ID
    /// </summary>
    public Category? GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);

    /// <summary>
    /// Adds new category
    /// </summary>
    public void Add(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        category.Id = _nextId++;
        _categories.Add(category);
        OnCategoriesChanged?.Invoke();
    }

    /// <summary>
    /// Updates existing category
    /// </summary>
    public void Update(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        var index = _categories.FindIndex(c => c.Id == category.Id);
        if (index >= 0)
        {
            _categories[index] = category;
            OnCategoriesChanged?.Invoke();
        }
    }

    /// <summary>
    /// Deletes category by ID
    /// </summary>
    public void Delete(int id)
    {
        var removed = _categories.RemoveAll(c => c.Id == id);
        if (removed > 0)
        {
            OnCategoriesChanged?.Invoke();
        }
    }
}
