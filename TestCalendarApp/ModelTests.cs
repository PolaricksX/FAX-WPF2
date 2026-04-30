using Calendar;
using FAX;
using Xunit;

namespace TestCalendarApp;

public class HomeCalendarTests
{
    [Fact]
    public void HomeCalendar_Constructor_CreatesInstance()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);

        // Assert
        Assert.NotNull(calendar);
        Assert.IsType<HomeCalendar>(calendar);
    }

    [Fact]
    public void HomeCalendar_Categories_Property_IsAvailable()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);

        // Assert
        Assert.NotNull(calendar.categories);
    }

    [Fact]
    public void HomeCalendar_Events_Property_IsAvailable()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);

        // Assert
        Assert.NotNull(calendar.events);
    }
}

public class CategoriesTests
{
    [Fact]
    public void Categories_List_ReturnsExpectedCategories()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);

        // Act
        var categories = calendar.categories.List();

        // Assert
        Assert.NotNull(categories);
    }

    [Fact]
    public void Categories_Add_WithDescriptionAndType_AddsCategory()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);
        var description = "Test Category";
        var type = Category.CategoryType.Event;

        // Act
        calendar.categories.Add(description, type);

        // Assert
        var categories = calendar.categories.List();
        Assert.Contains(categories, category =>
            category.Description == description && category.CategoryType == type);
    }
}

public class EventsTests
{
    [Fact]
    public void Events_List_ReturnsExpectedEvents()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);

        // Act
        var events = calendar.events.List();

        // Assert
        Assert.NotNull(events);
    }

    [Fact]
    public void Events_Add_AddsEvent()
    {
        // Arrange
        var calendar = new HomeCalendar("test.calendar", true);
        calendar.categories.Add("Test Category", Category.CategoryType.Event);

        // Act
        calendar.events.Add(1, 60, "2026-04-29 14:00:00", "Test event");

        // Assert
        var events = calendar.events.List();
        Assert.NotEmpty(events);
    }
}
