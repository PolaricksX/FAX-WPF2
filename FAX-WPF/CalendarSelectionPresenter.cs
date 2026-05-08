using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    internal class CalendarSelectionPresenter
    {
        private readonly HomeCalendar _model;
        public CalendarSelectionPresenter(ICalendarSelectionView v, HomeCalendar m)
        {
            _model = m;
        }

        public List<Event> GetEvents()
        {
            return _model.events.List(  );
        }

        public static double GetTotalDuration(List<Event> items)
        {
            if (items.Count == 0)
            {
                return 0;
            }

            double total = 0;
            foreach (var item in items)
            {
                total += item.DurationInMinutes;
            }

            return total;
        }
    }
}
