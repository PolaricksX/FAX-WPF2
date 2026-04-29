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
    internal class CategoryPresenter
    {
        private readonly ICategoryView _view;
        private readonly HomeCalendar _model;
        public CategoryPresenter(ICategoryView v, HomeCalendar m)
        {
            _view = v;
            _model = m;
        }

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
