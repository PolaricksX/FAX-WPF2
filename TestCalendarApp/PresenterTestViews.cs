using System;
using System.Collections.Generic;
using Calendar;
using FAX_WPF;

namespace TestCalendarApp;

public class MainViewTest : IMainView
{
    public bool ShowMessageCalled { get; private set; }
    public string? LastMessage { get; private set; }

    public void ShowMessage(string message)
    {
        ShowMessageCalled = true;
        LastMessage = message;
    }
}

public class EventViewTest : IEventView
{
    public int CategoryId { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime StartDateTime { get; set; }
    public string Details { get; set; } = string.Empty;

    public bool ShowMessageCalled { get; private set; }
    public string? LastMessage { get; private set; }

    public void ShowMessage(string message)
    {
        ShowMessageCalled = true;
        LastMessage = message;
    }
}

public class CategoryViewTest : ICategoryView
{
    public string Description { get; set; } = string.Empty;
    public Category.CategoryType SelectedCategoryType { get; set; }

    public bool ShowMessageCalled { get; private set; }
    public string? LastMessage { get; private set; }

    public void ShowMessage(string message)
    {
        ShowMessageCalled = true;
        LastMessage = message;
    }
}
