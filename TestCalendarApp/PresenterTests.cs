using FAX_WPF;
using TestCalendarApp;
using Xunit;

namespace TestCalendarApp;

public class MainPresenterTests
{
    [Fact]
    public void MainPresenter_CreatesInstance()
    {
        // Arrange
            var mainPresenter = CreateMainPresenter("MainPresenterTest_CreatesInstance");

        // Assert
        Assert.NotNull(mainPresenter);
        Assert.IsType<MainPresenter>(mainPresenter);
    }

    [Fact]
    public void MainPresenter_GetEventsPresenter_ReturnsEventsPresenter()
    {
        // Arrange
            var mainPresenter = CreateMainPresenter("MainPresenterTest_GetEventsPresenter");
        var eventView = CreateSampleEventView();

        // Act
        var eventsPresenter = mainPresenter.GetEventsPresenter(eventView);

        // Assert
        Assert.NotNull(eventsPresenter);
        Assert.IsType<EventsPresenter>(eventsPresenter);
    }

    [Fact]
    public void MainPresenter_GetCategoryPresenter_ReturnsCategoryPresenter()
    {
        // Arrange
        var mainPresenter = CreateMainPresenter("MainPresenterTest_GetCategoryPresenter");
        var categoryView = new CategoryViewTest();

        // Act
        var categoryPresenter = mainPresenter.GetCategoryPresenter(categoryView);

        // Assert
        Assert.NotNull(categoryPresenter);
        Assert.IsType<CategoryPresenter>(categoryPresenter);
    }

    private static MainPresenter CreateMainPresenter(string filename)
    {
        return new MainPresenter(new MainViewTest(), filename + ".calendar", true);
    }

    private static EventViewTest CreateSampleEventView()
    {
        return new EventViewTest
        {
            CategoryId = 1,
            DurationMinutes = 60,
            StartDateTime = new DateTime(2026, 4, 29, 14, 0, 0, DateTimeKind.Unspecified),
            Details = "Test event"
        };
    }
}