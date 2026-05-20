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
        /// Shows a confirmation dialog on success and closes the window.
        /// </summary>
        /// <remarks>If an error occurs during the save operation, an appropriate message is displayed to
        /// the user. This method relies on the current state of the view for category
        /// details.
        /// </remarks>
        /// <exception cref="SQLiteException">Thrown when a database-level error occurs during the save operation.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the save operation is performed in an invalid state.</exception>
        /// <exception cref="ArgumentException">Thrown when the category description or type is invalid.</exception>
        public void SaveCategory()
        {
            try
            {
                // Validate description
                if (string.IsNullOrWhiteSpace(_view.Description))
                {
                    _view.ShowMessage("Please provide a category description.");
                    return;
                }

                // Validate category type selection
                if (_view.SelectedCategoryType == null)
                {
                    _view.ShowMessage("Please select a category type.");
                    return;
                }

                _model.categories.Add(
                    _view.Description,
                    _view.SelectedCategoryType);

                System.Windows.MessageBox.Show(
                    "Category saved successfully!",
                    "Success",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);

                // Close the window
                if (_view is System.Windows.Window window)
                {
                    window.Close();
                }
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                _view.ShowMessage($"Database error: Unable to save category. {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowMessage($"Unable to save category. {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                _view.ShowMessage(ex.Message);
            }
            catch (Exception)
            {
                _view.ShowMessage("An unexpected error occurred while saving the category. Please try again.");
            }
        }

        /// <summary>
        /// Retrieves the list of categories from the model.
        /// </summary>
        /// <returns>A list of Category objects.</returns>
        public List<Category> GetCategories()
        {
            return _model.categories.List();
        }

        /// <summary>
        /// Deletes the selected category if no events are associated with it.
        /// </summary>
        /// <param name="categoryToDelete">The category to delete.</param>
        /// <returns>True if delete succeeds; otherwise false.</returns>
        public bool DeleteCategory(Category categoryToDelete)
        {
            if (categoryToDelete == null)
            {
                _view.ShowMessage("Please select a category to delete.");
                return false;
            }

            try
            {
                var hasEvents = _model.events.List().Any(e => e.Category == categoryToDelete.Id);
                if (hasEvents)
                {
                    _view.ShowMessage("This category cannot be deleted because events are associated with it.");
                    return false;
                }

                _model.categories.Delete(categoryToDelete.Id);
                _view.ShowMessage("Category deleted successfully.");
                return true;
            }
            catch (Exception)
            {
                _view.ShowMessage("Unable to delete category. Please try again.");
                return false;
            }
        }

    }
}
