using Calendar;
using FAX;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    /// <summary>
    /// Coordinates the logic between the view of category and the home calendar model.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// ICategoryView view = new CreateCategories(mainPresenter);
    /// 
    /// HomeCalendar model = new HomeCalendar("calendar.db");
    /// 
    /// CategoryPresenter presenter = new CategoryPresenter(view, model);
    /// ]]>
    /// </code>
    /// </example>
    internal class CategoryPresenter
    {
        private readonly ICategoryView _view;
        private readonly HomeCalendar _model;

        /// <summary>
        /// Initializes a new instance of the CategoryPresenter class with the specified view and model.
        /// </summary>
        /// <param name="v">The view interface used to display and interact with category data. Cannot be null.</param>
        /// <param name="m">The HomeCalendar model that provides category data and business logic. Cannot be null.</param>
        public CategoryPresenter(ICategoryView v, HomeCalendar m)
        {
            _view = v;
            _model = m;
        }

        /// <summary>
        /// Attempts to save a new category using the current values from the view.
        /// </summary>
        /// <remarks>If an error occurs during the save operation, an appropriate message is displayed to
        /// the user and the method returns false. This method relies on the current state of the view for category
        /// details.
        /// </remarks>
        /// <returns>true if the category is saved successfully; otherwise, false.</returns>
        /// <exception cref="SQLiteException">Thrown when a database-level error occurs during the save operation.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the save operation is performed in an invalid state.</exception>
        /// <exception cref="ArgumentException">Thrown when the category description or type is invalid.</exception>
        public bool SaveCategory()
        {
            try
            {
                _model.categories.Add(
                    _view.Description,
                    _view.SelectedCategoryType);

                _view.ShowMessage("Category saved successfully!");
                return true;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                _view.ShowMessage($"Database error: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowMessage($"Save failed: {ex.Message}");
                return false;
            }
            catch (ArgumentException ex)
            {
                _view.ShowMessage(ex.Message);
                return false;
            }
            catch (Exception)
            {
                _view.ShowMessage("An unexpected error occurred while saving the category.");
                return false;
            }
        }

    }
}
