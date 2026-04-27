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

    }
}
