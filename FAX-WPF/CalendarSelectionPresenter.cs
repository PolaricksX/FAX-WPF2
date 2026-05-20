using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    internal enum SummaryMode
    {
        None,
        Category,
        Month,
        CategoryMonth
    }

    public class SummaryRow
    {
        public string Label { get; set; }
        public double TotalMinutes { get; set; }
    }

    internal class CalendarSelectionPresenter
    {
        private readonly HomeCalendar _model;
        public CalendarSelectionPresenter(ICalendarSelectionView v, HomeCalendar m)
        {
            _model = m;
        }

        public List<Event> GetEvents()
        {
            return _model.events.List(  );
        }

        public List<Category> GetCategories()
        {
            return _model.categories.List();
        }

        public List<Event> GetFilteredEvents(DateTime? startDate, DateTime? endDate, TimeSpan? startTime, TimeSpan? endTime, int? categoryId)
        {
            var items = _model.events.List();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                items = items.Where(item => item.Category == categoryId.Value).ToList();
            }

            if (startDate.HasValue)
            {
                var start = startDate.Value.Date;
                items = items.Where(item => item.StartDateTime.Date >= start).ToList();
            }

            if (endDate.HasValue)
            {
                var end = endDate.Value.Date;
                items = items.Where(item => item.StartDateTime.Date <= end).ToList();
            }

            if (startTime.HasValue)
            {
                var start = startTime.Value;
                items = items.Where(item => item.StartDateTime.TimeOfDay >= start).ToList();
            }

            if (endTime.HasValue)
            {
                var end = endTime.Value;
                items = items.Where(item => item.StartDateTime.TimeOfDay <= end).ToList();
            }

            return items;
        }

        public List<SummaryRow> GetSummaryRows(List<Event> items, SummaryMode mode)
        {
            if (items == null || items.Count == 0 || mode == SummaryMode.None)
            {
                return new List<SummaryRow>();
            }

            var categoryLookup = _model.categories.List()
                .ToDictionary(category => category.Id, category => category.Description);

            switch (mode)
            {
                case SummaryMode.Category:
                    return items
                        .GroupBy(item => item.Category)
                        .OrderBy(group => group.Key)
                        .Select(group => new SummaryRow
                        {
                            Label = categoryLookup.TryGetValue(group.Key, out var name)
                                ? name
                                : $"Category {group.Key}",
                            TotalMinutes = group.Sum(item => item.DurationInMinutes)
                        })
                        .ToList();

                case SummaryMode.Month:
                    return items
                        .GroupBy(item => new { item.StartDateTime.Year, item.StartDateTime.Month })
                        .OrderBy(group => group.Key.Year)
                        .ThenBy(group => group.Key.Month)
                        .Select(group => new SummaryRow
                        {
                            Label = $"{group.Key.Year:D4}-{group.Key.Month:D2}",
                            TotalMinutes = group.Sum(item => item.DurationInMinutes)
                        })
                        .ToList();

                case SummaryMode.CategoryMonth:
                    return items
                        .GroupBy(item => new { item.StartDateTime.Year, item.StartDateTime.Month, item.Category })
                        .OrderBy(group => group.Key.Year)
                        .ThenBy(group => group.Key.Month)
                        .ThenBy(group => group.Key.Category)
                        .Select(group => new SummaryRow
                        {
                            Label = $"{group.Key.Year:D4}-{group.Key.Month:D2} - " +
                                    (categoryLookup.TryGetValue(group.Key.Category, out var name)
                                        ? name
                                        : $"Category {group.Key.Category}"),
                            TotalMinutes = group.Sum(item => item.DurationInMinutes)
                        })
                        .ToList();
            }

            return new List<SummaryRow>();
        }

        public static double GetTotalDuration(List<Event> items)
        {
            if (items.Count == 0)
            {
                return 0;
            }

            double total = 0;
            foreach (var item in items)
            {
                total += item.DurationInMinutes;
            }

            return total;
        }

        /// <summary>
        /// Deletes the specified event from the database.
        /// </summary>
        /// <param name="eventToDelete">The event to delete.</param>
        /// <returns>True if the delete succeeds; otherwise false.</returns>
        public bool DeleteEvent(Event eventToDelete)
        {
            if (eventToDelete == null)
            {
                return false;
            }

            try
            {
                _model.events.Delete(eventToDelete.Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
