using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    public interface IEventView
    {
        int CategoryId { get; set; }
        int DurationMinutes { get; set; }
        DateTime StartDateTime { get; set; }
        string Details { get; set; }
        void ShowMessage(string message);
    }
}
