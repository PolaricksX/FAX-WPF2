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

        /// <summary>
        /// Main presenter responsible for coordinating between the UI (Views) and the model (HomeCalendar)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// MainPresenter presenter = new MainPresenter(mainView, "calendar.json", true);
        /// ]]>
        /// </example>

        /// <summary>
        /// Reference to the main view interface.
        /// </summary>
        public IMainView MainView { get; }
        private readonly HomeCalendar _model;

        /// Initializes a new instance of the MainPresenter class.
        /// </summary>
        /// <param name="mainView">The main view interface implementation.</param>
        /// <param name="filename">The data source file used by the calendar model.</param>
        /// <param name="db">Indicates whether the model should use a database instead of file storage.</param>
        /// <example>
        /// <![CDATA[
        /// MainPresenter presenter = new MainPresenter(mainView, "data.json", false);
        /// ]]>
        /// </example>
        public MainPresenter(IMainView mainView, string filename, bool db)
        {
            MainView = mainView;
            _model = new HomeCalendar(filename, db);
        }



        internal EventsPresenter GetEventsPresenter(IEventView ev)
        {
            return new EventsPresenter(ev, _model);
        }

        internal CategoryPresenter GetCategoryPresenter(ICategoryView ev)
        {
            return new CategoryPresenter(ev, _model);
        }

        internal CalendarSelectionPresenter GetCalendarSelection(ICalendarSelectionView ev)
        {
            return new CalendarSelectionPresenter(ev, _model);
        }




    }

}
                                            