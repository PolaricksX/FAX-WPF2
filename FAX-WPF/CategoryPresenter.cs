using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    internal class CategoryPresenter
    {
        private ICategoryView _view;
        private HomeCalendar _model;
        public CategoryPresenter(ICategoryView v, HomeCalendar m)
        {
            _view = v;
            _model = m;
        }

        public void SaveCategory()
        {
            try
            {
                _model.categories.Add(
                    _view.Description,
                    _view.SelectedCategoryType.ToString());

                _view.ShowMessage("Category saved successfully!");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error saving category: {ex.Message}");
            }

        }

    }
}
