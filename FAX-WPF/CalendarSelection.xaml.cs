using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calendar;

namespace FAX_WPF
{
    /// <summary>
    /// Provides interaction logic for the CalendarSelection window.
    /// </summary>
    /// <remarks>
    /// It relies on a MainPresenter to handle the underlying data logic.
    /// </remarks>
    public partial class CalendarSelections : Window, ICalendarSelectionView
    {
        private sealed class FilterInputControls
        {
            public FilterInputControls(DatePicker startDatePicker, DatePicker endDatePicker, TextBox startTimeBox, TextBox endTimeBox, ComboBox categoryCombo)
            {
                StartDatePicker = startDatePicker;
                EndDatePicker = endDatePicker;
                StartTimeBox = startTimeBox;
                EndTimeBox = endTimeBox;
                CategoryCombo = categoryCombo;
            }

            public DatePicker StartDatePicker { get; }
            public DatePicker EndDatePicker { get; }
            public TextBox StartTimeBox { get; }
            public TextBox EndTimeBox { get; }
            public ComboBox CategoryCombo { get; }
        }

        private sealed class FilterCategoryOption
        {
            public int? CategoryId { get; set; }
            public string Description { get; set; } = string.Empty;
        }

        private readonly MainPresenter _mainPresenter;
        private readonly CalendarSelectionPresenter _calPresenter;

        /// <summary>
        /// Creates a new instance of CalendarSelections class.
        /// </summary>
        /// <param name="mainPresenter">The MainPresenter used to manage logic.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// var presenter = new MainPresenter();
        /// 
        /// var calendarWindow = new CalendarSelections(presenter);
        /// 
        /// calendarWindow.Show();
        /// ]]>
        /// </code>
        /// </example>
        public CalendarSelections(MainPresenter mainPresenter)
        {
            InitializeComponent();
            _mainPresenter = mainPresenter;
            _calPresenter = mainPresenter.GetCalendarSelection(this);
            Loaded += CalendarSelections_Loaded;
        }

        private void CalendarSelections_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilterOptions();
            cmbSummaryMode.SelectedIndex = 0;
            RefreshEvents();
        }

        /// <summary>
        /// Handles the close button click.
        /// </summary>
        private void btnWindowClose_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshEvents()
        {
            var items = _calPresenter.GetEvents();
            SetEvents(items);
            UpdateSummary(items);
        }

        private void LoadFilterOptions()
        {
            cmbFilterCategory.Items.Clear();
            cmbFilterCategory.Items.Add(new FilterCategoryOption
            {
                CategoryId = null,
                Description = "All categories"
            });

            foreach (var category in _calPresenter.GetCategories())
            {
                cmbFilterCategory.Items.Add(new FilterCategoryOption
                {
                    CategoryId = category.Id,
                    Description = category.Description
                });
            }

            cmbFilterCategory.DisplayMemberPath = "Description";
            cmbFilterCategory.SelectedIndex = 0;
        }

        private void BtnCreateCategory_Click(object sender, RoutedEventArgs e)
        {
            var createCategories = new CreateCategories(_mainPresenter);
            createCategories.ShowDialog();
        }

        private void BtnCreateEvent_Click(object sender, RoutedEventArgs e)
        {
            var createEvents = new CreateEvents(_mainPresenter);
            createEvents.ShowDialog();
            RefreshEvents();
        }

        private void BtnDeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (dgEvents.SelectedItem is not Event selectedEvent)
            {
                MessageBox.Show("Please select an event to delete.");
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this event?",
                "Delete Event",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (_calPresenter.DeleteEvent(selectedEvent))
            {
                MessageBox.Show("Event deleted successfully.");
                RefreshEvents();
            }
            else
            {
                MessageBox.Show("Unable to delete the event. Please try again.");
            }
        }

        private void dgEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgEvents.SelectedItem is Event selectedEvent)
            {
                var createEvents = new CreateEvents(_mainPresenter);
                createEvents.LoadEventForEditing(selectedEvent);
                createEvents.ShowDialog();
                RefreshEvents();
            }
        }

        public void SetEvents(List<Event> items)
        {
            dgEvents.ItemsSource = items;
        }

        public void SetSummary(int eventCount, double totalBusyTime)
        {
            txtEventCount.Text = eventCount.ToString();
            txtBusyTime.Text = totalBusyTime.ToString("0.##");  
        }

        public void SetSummaryRows(List<SummaryRow> rows)
        {
            dgSummary.ItemsSource = rows;
        }

        private void UpdateSummary(List<Event> items)
        {
            SetSummary(items.Count, CalendarSelectionPresenter.GetTotalDuration(items));
            SetSummaryRows(_calPresenter.GetSummaryRows(items, GetSelectedSummaryMode(cmbSummaryMode)));
        }

        private static SummaryMode GetSelectedSummaryMode(ComboBox summaryMode)
        {
            if (summaryMode.SelectedItem is ComboBoxItem comboItem)
            {
                switch (comboItem.Content?.ToString())
                {
                    case "By Category":
                        return SummaryMode.Category;
                    case "By Month":
                        return SummaryMode.Month;
                    case "By Category + Month":
                        return SummaryMode.CategoryMonth;
                }
            }

            return SummaryMode.None;
        }

        private void BtnApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var controls = new FilterInputControls(dpFilterStartDate, dpFilterEndDate, txtFilterStartTime, txtFilterEndTime, cmbFilterCategory);

            if (!TryGetFilterInputs(controls, out var startDate, out var endDate, out var startTime, out var endTime, out var categoryId))
            {
                return;
            }

            var items = _calPresenter.GetFilteredEvents(startDate, endDate, startTime, endTime, categoryId);
            SetEvents(items);
            UpdateSummary(items);
        }

        private void BtnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            dpFilterStartDate.SelectedDate = null;
            dpFilterEndDate.SelectedDate = null;
            txtFilterStartTime.Text = string.Empty;
            txtFilterEndTime.Text = string.Empty;
            cmbFilterCategory.SelectedIndex = 0;

            RefreshEvents();
        }

        private void CmbSummaryMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEvents.ItemsSource is List<Event> items)
            {
                UpdateSummary(items);
            }
            else
            {
                RefreshEvents();
            }
        }

        private static bool TryGetFilterInputs(FilterInputControls controls, out DateTime? startDate, out DateTime? endDate,
            out TimeSpan? startTime, out TimeSpan? endTime, out int? categoryId)
        {
            startDate = controls.StartDatePicker.SelectedDate;
            endDate = controls.EndDatePicker.SelectedDate;
            startTime = null;
            endTime = null;
            categoryId = null;

            if (startDate.HasValue && endDate.HasValue && startDate.Value.Date > endDate.Value.Date)
            {
                MessageBox.Show("Start date must be before end date.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(controls.StartTimeBox.Text))
            {
                if (!TryParseTime(controls.StartTimeBox.Text, out var parsed))
                {
                    MessageBox.Show("Start time must be in HH:mm format.");
                    return false;
                }

                startTime = parsed;
            }

            if (!string.IsNullOrWhiteSpace(controls.EndTimeBox.Text))
            {
                if (!TryParseTime(controls.EndTimeBox.Text, out var parsed))
                {
                    MessageBox.Show("End time must be in HH:mm format.");
                    return false;
                }

                endTime = parsed;
            }

            if (startTime.HasValue && endTime.HasValue && startTime.Value > endTime.Value)
            {
                MessageBox.Show("Start time must be before end time.");
                return false;
            }

            if (controls.CategoryCombo.SelectedItem is FilterCategoryOption categoryOption)
            {
                categoryId = categoryOption.CategoryId;
            }

            return true;
        }

        private static bool TryParseTime(string input, out TimeSpan time)
        {
            return TimeSpan.TryParseExact(input, "hh\\:mm", System.Globalization.CultureInfo.InvariantCulture, out time)
                || TimeSpan.TryParseExact(input, "h\\:mm", System.Globalization.CultureInfo.InvariantCulture, out time)
                || TimeSpan.TryParseExact(input, "HH\\:mm", System.Globalization.CultureInfo.InvariantCulture, out time);
        }
    }
}
