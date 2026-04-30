using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    /// <summary>
    /// Represents a view for displaying and editing event information.
    /// </summary>
    public interface IEventView
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated category.
        /// </summary>
        int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the duration of the event, in minutes.
        /// </summary>
        int DurationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the event starts.
        /// </summary>
        DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the detailed description or additional information associated with the event.
        /// </summary>
        string Details { get; set; }

        /// <summary>
        /// Displays the specified message to the user.
        /// </summary>
        /// <param name="message">The message text to display. Cannot be null.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// 
        /// view.ShowMessage("Event created!");
        /// 
        /// ]]>
        /// </code>
        /// </example>
        void ShowMessage(string message);
    }
}
