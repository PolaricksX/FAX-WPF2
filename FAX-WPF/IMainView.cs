using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    /// <summary>
    /// Represents the main view interface for displaying messages to the user.
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// Displays the specified message to the user.
        /// </summary>
        /// <param name="message">The message text to display. Cannot be null.</param>
        void ShowMessage(string message);
    }
}
