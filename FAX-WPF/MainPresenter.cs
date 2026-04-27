using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    internal class MainPresenter
    {
        private IMainView _mainPresenter;
        private HomeCalendar _model;

        public MainPresenter(IMainView mainView)
        {
            _mainPresenter = mainView;
            _model = new HomeCalendar("Default", true);
        }

        public EventsPresenter GetEventsPresenter(IEventView ev)
        {
            return new EventsPresenter(ev, _model);
        }

        
       
    }
}
