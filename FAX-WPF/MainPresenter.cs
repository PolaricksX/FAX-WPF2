using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FAX_WPF
{
    public class MainPresenterWindow

    {
        
        private HomeCalendar _model;
        private IMainView _view;

        internal MainPresenterWindow(IMainView view, string fileName,bool isnewDB)
        {
            _view = view;
            _model = new HomeCalendar(fileName, isnewDB);
        }
        
        internal EventsPresenter CreateEventsPresenter(IEventView view)
        {
            return new EventsPresenter();   
        }

    }

}
                                            