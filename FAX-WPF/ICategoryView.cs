using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace FAX_WPF
{
    /// <summary>
    /// Represents a view for displaying and editing category information.
    /// </summary>
    public interface ICategoryView
    {
        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the currently selected category type.
        /// </summary>
        Calendar.Category.CategoryType SelectedCategoryType { get; set; }

        /// <summary>
        /// Displays the specified message to the user.
        /// </summary>
        /// <param name="message">The message to display. Cannot be null.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// 
        /// view.ShowMessage("Category created!");
        /// 
        /// ]]>
        /// </code>
        /// </example>
        void ShowMessage(string message);
    }
}
