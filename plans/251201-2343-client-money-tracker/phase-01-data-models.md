# Phase 01: Data Models

## Context Links

- **Parent Plan:** [plan.md](plan.md)
- **Dependencies:** None (foundation phase)
- **References:**
  - [Brainstorm Report](../reports/brainstorm-251201-client-side-money-tracker.md)
  - [Code Standards](../../docs/code-standards.md)

## Overview

**Date:** 2025-12-01
**Description:** Create data models for transactions and categories
**Priority:** P0 (Foundation)
**Implementation Status:** ✓ Completed (2025-12-02)
**Review Status:** ✓ Approved (Code-Reviewer-251202)

## Key Insights

- Models use nullable reference types (#nullable enable)
- Validation attributes for future form validation
- Simple POCOs (Plain Old CLR Objects) - no EF Core yet
- Navigation property optional (Category?) for flexibility
- Enum for TransactionType ensures type safety

## Requirements

### Functional
- Transaction model with all required fields
- Category model for user-defined categories
- TransactionType enum for Income/Expense distinction
- Support for future database migration (Id fields)

### Non-Functional
- File size < 200 lines per file
- XML comments on public properties
- Nullable reference types enabled
- Validation attributes where appropriate
- Modern C# syntax (init properties, default values)

## Architecture

### Data Model Hierarchy

```
TransactionType (enum)
  ↓
Category (independent entity)
  ↓
Transaction (references Category via CategoryId)
```

### Relationships

```
Transaction.CategoryId → Category.Id (foreign key relationship)
Transaction.Category? → Optional navigation property
```

## Related Code Files

### Files to Create
1. `MyMoneySaver/MyMoneySaver/Models/TransactionType.cs`
2. `MyMoneySaver/MyMoneySaver/Models/Category.cs`
3. `MyMoneySaver/MyMoneySaver/Models/Transaction.cs`

### Existing Files (Reference)
- No existing models - creating folder structure from scratch

## Implementation Steps

### Step 1: Create Models Folder
```bash
cd MyMoneySaver/MyMoneySaver
mkdir Models
```

### Step 2: Create TransactionType.cs (~10 lines)

**File:** `Models/TransactionType.cs`

```csharp
namespace MyMoneySaver.Models;

/// <summary>
/// Transaction type classification
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Money going out (default)
    /// </summary>
    Expense = 0,

    /// <summary>
    /// Money coming in
    /// </summary>
    Income = 1
}
```

**Key Points:**
- Expense is default (0 value)
- XML comments explain each value
- Simple two-value enum (YAGNI)

### Step 3: Create Category.cs (~30 lines)

**File:** `Models/Category.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace MyMoneySaver.Models;

/// <summary>
/// Category for transaction classification
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category display name
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Material Design icon name (e.g., "restaurant", "directions_car")
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Icon { get; set; } = "category";

    /// <summary>
    /// Hex color code (e.g., "#ff9800")
    /// </summary>
    [Required]
    [StringLength(7, MinimumLength = 7)]
    [RegularExpression(@"^#[0-9a-fA-F]{6}$", ErrorMessage = "Color must be valid hex format (#RRGGBB)")]
    public string Color { get; set; } = "#1976d2";
}
```

**Key Points:**
- Required validation on all properties
- StringLength constraints prevent data issues
- RegularExpression validates hex color format
- Default values for Icon and Color
- Name initialized to empty string (non-nullable)

### Step 4: Create Transaction.cs (~40 lines)

**File:** `Models/Transaction.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace MyMoneySaver.Models;

/// <summary>
/// Financial transaction record
/// </summary>
public class Transaction
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Transaction amount (positive value)
    /// </summary>
    [Required]
    [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Associated category ID
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Transaction description/note
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Transaction date
    /// </summary>
    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    /// <summary>
    /// Transaction type (Income or Expense)
    /// </summary>
    [Required]
    public TransactionType Type { get; set; } = TransactionType.Expense;

    /// <summary>
    /// Navigation property to Category (optional, for display purposes)
    /// </summary>
    public Category? Category { get; set; }
}
```

**Key Points:**
- Amount uses decimal for precision (no float/double)
- Range validation on Amount (0.01 to 1M)
- CategoryId required (Range validates > 0)
- Description limited to 200 chars
- Date defaults to today
- Type defaults to Expense (most common)
- Category navigation property nullable (not always loaded)

## Todo List

- [x] Create Models folder in MyMoneySaver/MyMoneySaver
- [x] Create TransactionType.cs with enum definition
- [x] Create Category.cs with validation attributes
- [x] Create Transaction.cs with navigation property
- [x] Verify all files compile without errors
- [x] Check file sizes (should be under 200 lines)
- [x] Verify XML comments on public members
- [x] Ensure nullable reference types work correctly

## Success Criteria

### Compilation
- [ ] Project builds without errors
- [ ] No compiler warnings about nullability
- [ ] Validation attributes recognized

### Code Quality
- [x] TransactionType.cs < 20 lines ✓
- [x] Category.cs < 40 lines ✓
- [x] Transaction.cs < 50 lines ✓
- [x] XML comments on all public members ✓
- [x] Validation attributes applied ✓
- [x] Nullable reference types enabled ✓

### Standards Compliance
- [x] PascalCase for class names ✓
- [x] PascalCase for property names ✓
- [x] Namespace follows folder structure ✓
- [x] No unnecessary complexity (KISS) ✓
- [x] No code duplication (DRY) ✓
- [x] Only essential properties (YAGNI) ✓

## Risk Assessment

### Risk 1: Namespace Issues
**Likelihood:** Low
**Impact:** Low
**Mitigation:** Use namespace MyMoneySaver.Models consistently

### Risk 2: Validation Attribute Compatibility
**Likelihood:** Low
**Impact:** Medium
**Mitigation:** Standard System.ComponentModel.DataAnnotations supported in .NET 10

### Risk 3: Nullable Reference Type Warnings
**Likelihood:** Medium
**Impact:** Low
**Mitigation:** Initialize strings to string.Empty, use nullable Category?

## Security Considerations

- **Input Validation:** Validation attributes will catch invalid input in UI
- **Range Limits:** Amount capped at 1M to prevent overflow issues
- **SQL Injection:** Not applicable (no database yet, in-memory only)
- **XSS Prevention:** Blazor auto-encodes output (safe by default)

## Next Steps

1. **After Completion:** Proceed to Phase 02 (Services)
2. **Services will reference these models:** Import MyMoneySaver.Models namespace
3. **Future Database:** These models ready for EF Core with minimal changes (add [Key] attributes if needed)

## Notes

- Models are "dumb" objects - no business logic
- Validation attributes will be used by MudForm in UI
- Navigation property (Category?) may remain null in service layer
- Decimal type chosen for Amount (precision critical for money)
- DateTime.Today default ensures valid date always present

---

## Review Report
**Report:** [code-reviewer-251202-phase01-data-models.md](../reports/code-reviewer-251202-phase01-data-models.md)

**Summary:** ✓ APPROVED - All 3 model files pass comprehensive code review. Zero critical issues. 100% standards compliance.

**Key Findings:**
- 0 critical issues
- 0 high-priority issues
- 0 medium-priority issues
- All success criteria passed (14/14)
- Build: Success (0 warnings)
- Security: Excellent (comprehensive validation)
- Performance: Optimal (POCO models)
- Architecture: Clean (proper separation)

**Status:** ✓ Complete and Approved
**Next Phase:** Phase 02 (Services) - Models ready for service layer consumption
