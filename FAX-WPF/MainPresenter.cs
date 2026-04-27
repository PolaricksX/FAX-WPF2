using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FAX_WPF
{
    public class MainPresenter
    {
        public IMainView _mainPresenter;
        private HomeCalendar _model;

        public MainPresenter(IMainView mainView, string filename, bool db)
        {
            _mainPresenter = mainView;
            _model = new HomeCalendar("Default", true);
        }

        internal EventsPresenter GetEventsPresenter(IEventView ev)
        {
            return new EventsPresenter(ev, _model);
        }
        
        

        
       
    }

}
                                            