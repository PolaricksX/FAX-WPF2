using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FAX_WPF
{
    /// <summary>
    /// Interaction logic for CreateCategories.xaml
    /// </summary>
    public partial class CreateCategories : Window, ICategoryView
    {
        private Calendar.Category.CategoryType _selectedCategoryType;
        private CategoryPresenter _catPresenter;
        private MainPresenter _mainPresenter;

        public CreateCategories(MainPresenter p)
        {
            InitializeComponent();
            _catPresenter = p.GetCategoryPresenter(this);
        }

        private void btnCreateCategory_Clicked(object sender, RoutedEventArgs e)
        {
            if (_catPresenter.SaveCategory())
            {
                Close();
            }
        }

        public string Description
        {
            get
            {
                if (!(string.IsNullOrEmpty(txtDescription.Text)))
                {
                    return txtDescription.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                txtDescription.Text = value;
            }
        }

        public Calendar.Category.CategoryType SelectedCategoryType
        {
           get
            {
              return _selectedCategoryType;
            }
            set
            {
                _selectedCategoryType = value;

                switch (value)
                {
                    case Calendar.Category.CategoryType.Event:
                        cbEvent.IsChecked = true;
                        break;
                    case Calendar.Category.CategoryType.AllDayEvent:
                        cbAllDayEvent.IsChecked = true; 
                        break;
                    case Calendar.Category.CategoryType.Holiday:
                        cbHoliday.IsChecked = true;
                        break;
                    case Calendar.Category.CategoryType.Availability:
                        cbAvailability.IsChecked = true;
                        break;
                }
            }
        }

        private void CategoryCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb)
            {
                return;
            }

            switch (cb.Name)
            {
                case "cbEvent":
                    SelectedCategoryType = Calendar.Category.CategoryType.Event;
                    break;
                case "cbAllDayEvent":
                    SelectedCategoryType = Calendar.Category.CategoryType.AllDayEvent;
                    break;
                case "cbHoliday":
                    SelectedCategoryType = Calendar.Category.CategoryType.Holiday;
                    break;
                case "cbAvailability":
                    SelectedCategoryType = Calendar.Category.CategoryType.Availability;
                    break;
                default:
                    break;
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
