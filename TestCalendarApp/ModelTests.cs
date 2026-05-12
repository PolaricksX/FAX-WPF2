using FAX;
using Calendar;
public class HomeCalendarTests
{
    private static string GetTempFile(string name)
    {
        return System.IO.Path.Combine(System.IO.Path.GetTempPath(), name);
    }

    [Fact]
    public void HomeCalendar_Constructor_CreatesInstance()
    {
        string path = GetTempFile("HomeCalendarTest_Constructor.calendar");
        var calendar = new HomeCalendar(path, true);
        Assert.NotNull(calendar);
        Assert.IsType<HomeCalendar>(calendar);
    }

    [Fact]
    public void HomeCalendar_Categories_Property_IsAvailable()
    {
        string path = GetTempFile("HomeCalendarTest_Categories.calendar");
        var calendar = new HomeCalendar(path, true);
        Assert.NotNull(calendar.categories);
    }

    [Fact]
    public void HomeCalendar_Events_Property_IsAvailable()
    {
        string path = GetTempFile("HomeCalendarTest_Events.calendar");
        var calendar = new HomeCalendar(path, true);
        Assert.NotNull(calendar.events);
    }
}

public class CategoriesTests
{
    private static string GetTempFile(string name)
    {
        return System.IO.Path.Combine(System.IO.Path.GetTempPath(), name);
    }

    [Fact]
    public void Categories_List_ReturnsExpectedCategories()
    {
        string path = GetTempFile("CategoriesTests_List.calendar");
        var calendar = new HomeCalendar(path, true);
        var categories = calendar.categories.List();
        Assert.NotNull(categories);
    }

    public class EventsTests
    {
        private static string GetTempFile(string name)
        {
            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), name);
        }

        [Fact]
        public void Events_List_ReturnsExpectedEvents()
        {
            string path = GetTempFile("EventsTests_List.calendar");
            var calendar = new HomeCalendar(path, true);
            var events = calendar.events.List();
            Assert.NotNull(events);
        }

        [Fact]
        public void Events_Add_AddsEvent()
        {
            string path = GetTempFile("EventsTests_Add.calendar");
            var calendar = new HomeCalendar(path, true);
            calendar.categories.Add("Test Category", Category.CategoryType.Event);

            calendar.events.Add(1, 60, "2026-04-29 14:00:00", "Test event");

            var events = calendar.events.List();
            Assert.NotEmpty(events);
        }
    }
}