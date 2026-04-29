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

    }
}
