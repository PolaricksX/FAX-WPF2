using System.Collections.Generic;
using Calendar;

namespace FAX_WPF
{
    internal interface ICalendarSelectionView
    {
        void SetEvents(List<Event> items);
        void SetSummary(int eventCount, double totalBusyTime);
        void SetSummaryRows(List<SummaryRow> rows);
    }
}
