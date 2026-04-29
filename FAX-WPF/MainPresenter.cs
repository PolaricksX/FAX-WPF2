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
        public IMainView MainView { get; }
        private readonly HomeCalendar _model;

        public MainPresenter(IMainView mainView, string filename, bool db)
        {
            MainView = mainView;
            _model = new HomeCalendar("Default", true);
        }

        internal EventsPresenter GetEventsPresenter(IEventView ev)
        {
            return new EventsPresenter(ev, _model);
        }
        

        internal CategoryPresenter GetCategoryPresenter(ICategoryView ev)
        {
            return new CategoryPresenter(ev, _model);
        }




    }

}
                                            