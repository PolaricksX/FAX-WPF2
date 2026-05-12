using Calendar;
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
using System.Globalization;

namespace FAX_WPF
{
    /// <summary>
    /// Represents the view responsible for creating events.
    /// Acts as the UI layer and communicates with the EventsPresenter.
    /// </summary>
    /// <remarks>
    /// This window allows users to:
    /// - Select a category
    /// - Enter duration and date/time
    /// - Provide event details
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// // Creating and showing the CreateEvents window
    /// CreateEvents window = new CreateEvents(mainPresenter);
    /// window.Show();
    /// ]]>
    /// </example>
    public partial class CreateEvents : Window, IEventView
    {
        private readonly EventsPresenter _eventpresenter;
        private readonly MainPresenter _mainPresenter;
        private bool _hasUnsavedChanges = false;
        private bool _isEditMode = false;
        private Event _editingEvent = null;

        /// <summary>
        /// Initializes a new instance of the CreateEvents window.
        /// </summary>
        /// <param name="p">The main presenter instance.</param>
        /// <example>
        /// <![CDATA[
        /// CreateEvents createEvents = new CreateEvents(mainPresenter);
        /// createEvents.Show();
        /// ]]>
        /// </example>
        public CreateEvents(MainPresenter p)
        {
            InitializeComponent();
            _mainPresenter = p;
            _eventpresenter = p.GetEventsPresenter(this);

            cmbCategoryName.DisplayMemberPath = nameof(Category.Description);

            var categories = _eventpresenter.GetCategories();
            foreach (var category in categories)
            {
                cmbCategoryName.Items.Add(category);
            }

            // Set default date to today
            dpStartDate.SelectedDate = DateTime.Now;

            // Register event handlers for change tracking
            cmbCategoryName.SelectionChanged += Category_SelectionChanged;
            txtDuration.TextChanged += Input_TextChanged;
            dpStartDate.SelectedDateChanged += Date_SelectedDateChanged;
            txtDetails.TextChanged += Input_TextChanged;
        }

        /// <summary>
        /// Loads an existing event for editing.
        /// </summary>
        /// <param name="eventToEdit">The event to be edited</param>
        public void LoadEventForEditing(Event eventToEdit)
        {
            _isEditMode = true;
            _editingEvent = eventToEdit;
            _hasUnsavedChanges = false;

            // Update UI to reflect edit mode
            txtSubtitle.Text = "Edit Event";
            btnSave.Content = "Update Event";

            // Populate fields with event data
            CategoryId = eventToEdit.Category;
            DurationMinutes = (int)eventToEdit.DurationInMinutes;
            StartDateTime = eventToEdit.StartDateTime;
            Details = eventToEdit.Details;
        }

        /// <summary>
        /// Gets the event being edited (if in edit mode).
        /// </summary>
        public Event EditingEvent
        {
            get { return _editingEvent; }
        }

        /// <summary>
        /// Tracks when input fields are modified to set unsaved changes flag.
        /// </summary>
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            _hasUnsavedChanges = true;
        }

        /// <summary>
        /// Tracks when date is modified to set unsaved changes flag.
        /// </summary>
        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _hasUnsavedChanges = true;
        }

        /// <summary>
        /// Tracks when category selection changes and sets unsaved changes flag.
        /// </summary>
        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _hasUnsavedChanges = true;
            cmbCategoryName_SelectionChanged(sender, e);
        }

        /// <summary>
        /// Handles category selection changes and updates the category ID textbox.
        /// </summary>
        private void cmbCategoryName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategoryName.SelectedItem is Category category)
            {
                txtCategoryID.Text = category.Id.ToString();
            }
        }

        /// <summary>
        /// Handles the close button click.
        /// </summary>
        private void btnWindowClose_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the window closing event to check for unsaved changes.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_hasUnsavedChanges)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you want to discard them?",
                    "Unsaved Changes",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles date selection from date picker.
        /// </summary>
        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _hasUnsavedChanges = true;
        }

        /// <summary>
        /// Opens a calendar picker dialog for date selection.
        /// </summary>
        private void btnSelectDate_Clicked(object sender, RoutedEventArgs e)
        {
            // The DatePicker itself acts as the calendar picker
            // This button can be used to focus the date picker
            dpStartDate.Focus();
        }

        /// <summary>
        /// Attempts to save or update the event through the presenter.
        /// </summary>
        private void btnSave_Clicked(object sender, RoutedEventArgs e)
        {
            if (_isEditMode && _editingEvent != null)
            {
                _eventpresenter.UpdateEvent(_editingEvent);
            }
            else
            {
                _eventpresenter.SaveEvent();
            }
        }

        /// <summary>
        /// Prompts the user before cancelling event creation.
        /// </summary>
        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            const string message = "Are you sure you would like to cancel?";
            const string caption = "Closing Window";

            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _hasUnsavedChanges = false; // Don't prompt again
                this.Close();
            }
        }

        /// <summary>
        /// Opens the category creation window without closing the current one.
        /// </summary>
        private void btnCreateCategory_Clicked(object sender, RoutedEventArgs e)
        {
            CreateCategories createCategories = new CreateCategories(_mainPresenter);
            createCategories.ShowDialog();
            
            // Refresh categories list after category is created
            cmbCategoryName.Items.Clear();
            var categories = _eventpresenter.GetCategories();
            foreach (var category in categories)
            {
                cmbCategoryName.Items.Add(category);
            }
        }

        /// <summary>
        /// Clears all input fields for creating a new event.
        /// </summary>
        public void ClearFields()
        {
            cmbCategoryName.SelectedIndex = -1;
            txtCategoryID.Clear();
            txtDuration.Clear();
            dpStartDate.SelectedDate = DateTime.Now;
            txtDetails.Clear();
            _hasUnsavedChanges = false;
        }

        /// <summary>
        /// Gets or sets the selected category ID.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// // Getting selected category
        /// int id = view.CategoryId;
        ///
        /// // Setting selected category
        /// view.CategoryId = 3;
        /// ]]>
        /// </example>
        public int CategoryId
        {
            get
            {
                try
                {
                    if (cmbCategoryName.SelectedItem is Category category)
                    {
                        return category.Id;
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                try
                {
                    foreach (var item in cmbCategoryName.Items)
                    {
                        if (item is Category category && category.Id == value)
                        {
                            cmbCategoryName.SelectedItem = item;
                            break;
                        }
                    }
                }
                catch
                {
                    cmbCategoryName.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Gets or sets the duration of the event in minutes.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// view.DurationMinutes = 60; // Set duration
        /// int duration = view.DurationMinutes; // Get duration
        /// ]]>
        /// </example>
        public int DurationMinutes
        {
            get
            {
                try
                {
                    if (int.TryParse(txtDuration.Text, out int duration) && duration > 0)
                    {
                        return duration;
                    }
                    return 0;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    txtDuration.Text = value.ToString();
                }
                catch
                {
                    txtDuration.Text = "0";
                }
            }
        }

        /// <summary>
        /// Gets or sets the start date of the event.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// view.StartDateTime = DateTime.Now.AddDays(1);
        /// DateTime start = view.StartDateTime;
        /// ]]>
        /// </example>
        public DateTime StartDateTime
        {
            get
            {
                try
                {
                    DateTime selectedDate = dpStartDate.SelectedDate ?? DateTime.Now;
                    return new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0, DateTimeKind.Local);
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            set
            {
                try
                {
                    dpStartDate.SelectedDate = value.Date;
                }
                catch
                {
                    dpStartDate.SelectedDate = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Gets or sets the event details.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// view.Details = "Team meeting at 2 PM";
        /// string details = view.Details;
        /// ]]>
        /// </example>
        public string Details
        {
            get
            {
                try
                {
                    if (txtDetails.Text != null)
                    {
                        return txtDetails.Text;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    txtDetails.Text = value;
                }
                catch
                {
                    txtDetails.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Displays a message to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <example>
        /// <![CDATA[
        /// view.ShowMessage("Event saved successfully!");
        /// ]]>
        /// </example>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}

