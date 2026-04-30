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
    /// Represents a window that allows users to create and manage categories.
    /// </summary>
    /// <remarks>
    /// This window implements the ICategory interface 
    /// and interacts with the CategoryPresenter 
    /// to validate and save user-created categories.
    /// </remarks>
    public partial class CreateCategories : Window, ICategoryView
    {
        private Calendar.Category.CategoryType _selectedCategoryType;
        private CategoryPresenter _catPresenter;
        private bool _suppressCategoryCheckboxEvents;

        /// <summary>
        /// Initializes a new instance of the CreateCategories form using the specified main presenter.
        /// </summary>
        /// <param name="p">The main presenter that coordinates application logic and provides access to the category presenter.</param>
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

        /// <summary>
        /// Gets or sets the description text entered by the user.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the currently selected category type for the calendar entry.
        /// </summary>
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

        /// <summary>
        /// Displays a message box that shows the specified text to the user.
        /// </summary>
        /// <param name="message">The text to display in the message box.</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
