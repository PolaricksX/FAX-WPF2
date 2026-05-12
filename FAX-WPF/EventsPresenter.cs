using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    /// <summary>
    /// Coordinates interactions between the event view and the home calendar model.
    /// </summary>
    internal class EventsPresenter
    {
        private readonly IEventView _view;
        private readonly HomeCalendar _model;    

        /// <summary>
        /// Creates a new instance of the EventsPresenter class with the specified view and model.
        /// </summary>
        /// <param name="v">The event view interface that the presenter will interact with. Cannot be null.</param>
        /// <param name="m">The HomeCalendar model that provides event data. Cannot be null.</param>
        public EventsPresenter(IEventView v, HomeCalendar m)
        {
            _view = v;
            _model = m;
        }

        /// <summary>
        /// Tries to save the current event using the data provided by the view.
        /// Shows a confirmation dialog on success and prompts user for next action.
        /// </summary>
        /// <remarks>If the save operation fails, an error message is displayed to the user.
        /// Validation of input is delegated to the model.</remarks>
        public void SaveEvent()
        {
            try
            {
                // Validate category selection
                if (_view.CategoryId <= 0)
                {
                    _view.ShowMessage("Please select a valid category.");
                    return;
                }

                // Validate duration
                if (_view.DurationMinutes <= 0)
                {
                    _view.ShowMessage("Please enter a valid duration (must be greater than 0 minutes).");
                    return;
                }

                // Validate details
                if (string.IsNullOrWhiteSpace(_view.Details))
                {
                    _view.ShowMessage("Please provide event details.");
                    return;
                }

                _model.events.Add(
                    _view.CategoryId,
                    _view.DurationMinutes,
                    _view.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    _view.Details);

                var result = System.Windows.MessageBox.Show(
                    "Event saved successfully! Do you want to create another event?",
                    "Success",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Information);

                if (result == System.Windows.MessageBoxResult.No)
                {
                    // Close the window through the view
                    if (_view is System.Windows.Window window)
                    {
                        window.Close();
                    }
                }
                else
                {
                    // Clear fields for next event
                    if (_view is CreateEvents createEventsWindow)
                    {
                        createEventsWindow.ClearFields();
                    }
                }
            }
            catch(Exception ex)
            {
                _view.ShowMessage($"Unable to save event. Please check your input: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of all available categories.
        /// </summary>
        /// <returns>A list of Category objects representing the available categories.</returns>
        public List<Category> GetCategories()
        {
            return _model.categories.List();
        }

        /// <summary>
        /// Updates an existing event with new values from the view.
        /// Shows a confirmation dialog on success and closes the window.
        /// </summary>
        /// <remarks>If the update operation fails, an error message is displayed to the user.</remarks>
        /// <param name="eventToUpdate">The event object to update (must have valid EventId)</param>
        public void UpdateEvent(Event eventToUpdate)
        {
            try
            {
                // Validate category selection
                if (_view.CategoryId <= 0)
                {
                    _view.ShowMessage("Please select a valid category.");
                    return;
                }

                // Validate duration
                if (_view.DurationMinutes <= 0)
                {
                    _view.ShowMessage("Please enter a valid duration (must be greater than 0 minutes).");
                    return;
                }

                // Validate details
                if (string.IsNullOrWhiteSpace(_view.Details))
                {
                    _view.ShowMessage("Please provide event details.");
                    return;
                }

                // Call model's update method with all required parameters
                _model.events.UpdateEvent(
                    eventToUpdate.Id,
                    _view.CategoryId,
                    _view.DurationMinutes,
                    _view.StartDateTime,
                    _view.Details);

                System.Windows.MessageBox.Show(
                    "Event updated successfully!",
                    "Success",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);

                // Close the window
                if (_view is System.Windows.Window window)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Unable to update event. Please check your input: {ex.Message}");
            }
        }

    }
}
