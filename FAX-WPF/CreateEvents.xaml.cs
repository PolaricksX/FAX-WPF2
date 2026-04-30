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
        /// Closes the window.
        /// </summary>
        private void btnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Attempts to save the event through the presenter.
        /// Closes the window if successful.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// // Triggered when user clicks Save
        /// if (_eventpresenter.SaveEvent())
        /// {
        ///     Close();
        /// }
        /// ]]>
        /// </example>
        private void btnSave_Clicked(object sender, RoutedEventArgs e)
        {
            if (_eventpresenter.SaveEvent())
            {
                Close();
            }
        }

        /// <summary>
        /// Prompts the user before cancelling event creation.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// var result = MessageBox.Show(
        ///     "Are you sure you would like to cancel?",
        ///     "Closing Window",
        ///     MessageBoxButton.YesNo,
        ///     MessageBoxImage.Question
        /// );
        /// ]]>
        /// </example>
        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            // source: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.messageboxbuttons?view=windowsdesktop-10.0

            const string message = "Are you sure you would like to cancel?";
            const string caption = "Closing Window";

            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Opens the category creation window and closes the current one.
        /// </summary>
        private void btnCreateCategory_Clicked(object sender, RoutedEventArgs e)
        {
            CreateCategories createCategories = new CreateCategories(_mainPresenter);
            createCategories.Show();
            this.Close();
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
                if (cmbCategoryName.SelectedItem is Category category)
                {
                    return category.Id;
                }
                else
                {
                    return -1;
                }
            }
            set
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
        }

        /// <summary>
        /// Gets or sets the duration of the event in minutes.
        /// Returns 0 if parsing fails.
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
                if (int.TryParse(txtDuration.Text, out int duration))
                {
                    return duration;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                txtDuration.Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the start date and time of the event.
        /// Returns DateTime.Now if parsing fails.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// view.StartDateTime = DateTime.Now.AddHours(1);
        /// DateTime start = view.StartDateTime;
        /// ]]>
        /// </example>
        public DateTime StartDateTime
        {
            get
            {
                if (DateTime.TryParse(txtDateTime.Text, CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                {
                    return dt;
                }
                else
                {
                    return DateTime.Now;
                }
            }
            set
            {
                txtDateTime.Text = value.ToString();
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
                if (txtDetails.Text != null)
                {
                    return txtDetails.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                txtDetails.Text = value;
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
