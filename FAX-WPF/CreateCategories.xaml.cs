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
        private bool _suppressCategoryCheckboxEvents;

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
                _suppressCategoryCheckboxEvents = true;
                _selectedCategoryType = value;

                cbEvent.IsChecked = value == Calendar.Category.CategoryType.Event;
                cbAllDayEvent.IsChecked = value == Calendar.Category.CategoryType.AllDayEvent;
                cbHoliday.IsChecked = value == Calendar.Category.CategoryType.Holiday;
                cbAvailability.IsChecked = value == Calendar.Category.CategoryType.Availability;
                _suppressCategoryCheckboxEvents = false;
            }
        }

        private void CategoryCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (_suppressCategoryCheckboxEvents)
            {
                return;
            }

            if (sender is not CheckBox cb)
            {
                return;
            }

            if (cb.IsChecked == false)
            {
                _selectedCategoryType = default;
                return;
            }

            _suppressCategoryCheckboxEvents = true;
            cbEvent.IsChecked = cb.Name == nameof(cbEvent);
            cbAllDayEvent.IsChecked = cb.Name == nameof(cbAllDayEvent);
            cbHoliday.IsChecked = cb.Name == nameof(cbHoliday);
            cbAvailability.IsChecked = cb.Name == nameof(cbAvailability);
            _suppressCategoryCheckboxEvents = false;

            switch (cb.Name)
            {
                case nameof(cbEvent):
                    _selectedCategoryType = Calendar.Category.CategoryType.Event;
                    break;
                case nameof(cbAllDayEvent):
                    _selectedCategoryType = Calendar.Category.CategoryType.AllDayEvent;
                    break;
                case nameof(cbHoliday):
                    _selectedCategoryType = Calendar.Category.CategoryType.Holiday;
                    break;
                case nameof(cbAvailability):
                    _selectedCategoryType = Calendar.Category.CategoryType.Availability;
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
