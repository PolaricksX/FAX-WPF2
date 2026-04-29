using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    internal class EventsPresenter
    {
        private readonly IEventView _view;
        private readonly HomeCalendar _model;    
        public EventsPresenter(IEventView v, HomeCalendar m)
        {
           _view = v;
            _model = m;
        }

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
        public  List<Category> GetCategories()
        {
            return _model.categories.List();
        }

        

    }
}
