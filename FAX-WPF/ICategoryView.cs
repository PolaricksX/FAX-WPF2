using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    public interface ICategoryView
    {
        string Description { get; set; }
        CategoryType SelectedCategoryType { get; set; }
        void ShowMessage(string message);
    }

    public enum CategoryType
    {
        Event,
        AllDayEvent,
        Holiday,
        Availability
    }
}
