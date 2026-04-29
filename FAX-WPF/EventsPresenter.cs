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
        private IEventView _view;
        private HomeCalendar _model;    
        public EventsPresenter(IEventView v, HomeCalendar m)
        {
           _view = v;
            _model = m;
        }

        public void SaveEvent()
        {
            int tempid = 1;
            try
            {
                _model.events.Add(
                    tempid,
                    _view.DurationMinutes,
                    _view.StartDateTime.ToString("yyyy - MM - dd HH: mm:ss"),
                    _view.Details);

                _view.ShowMessage("Event saved successfully!");
            }
            catch(Exception ex)
            {
                _view.ShowMessage($"Error saving event: {ex.Message}");
            }

        }

        

    }
}
