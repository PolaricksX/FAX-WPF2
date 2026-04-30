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
        /// </summary>
        /// <remarks>If the save operation fails, an error message is displayed to the user.</remarks>
        /// <returns>true if the event is saved successfully. Otherwise, false.</returns>
        public bool SaveEvent()
        {
            try
            {
                _model.events.Add(
                    _view.CategoryId,
                    _view.DurationMinutes,
                    _view.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    _view.Details);

                _view.ShowMessage("Event saved successfully!");
                return true;
            }
            catch(Exception ex)
            {
                _view.ShowMessage($"Error saving event: {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// Retrieves a list of all available categories.
        /// </summary>
        /// <returns>A list of Category objects representing the available categories.</returns>
        public  List<Category> GetCategories()
        {
            return _model.categories.List();
        }

    }
}
