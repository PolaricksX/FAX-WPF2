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
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var mainPresenter = new MainPresenter();
    /// 
    /// var createCatWindow = new CreateCategories(mainPresenter);
    /// 
    /// createCatWindow.ShowDialog();
    /// ]]>
    /// </code>
    /// </example>
    public partial class CreateCategories : Window, ICategoryView
    {
        private Calendar.Category.CategoryType _selectedCategoryType;
        private CategoryPresenter _catPresenter;
        private bool _suppressCategoryCheckboxEvents;
        private bool _hasUnsavedChanges = false;

        /// <summary>
        /// Initializes a new instance of the CreateCategories form using the specified main presenter.
        /// </summary>
        /// <param name="p">The main presenter that coordinates application logic and provides access to the category presenter.</param>
        public CreateCategories(MainPresenter p)
        {
            InitializeComponent();
            _catPresenter = p.GetCategoryPresenter(this);

            // Register event handler for tracking changes
            txtDescription.TextChanged += Description_TextChanged;
        }

        /// <summary>
        /// Tracks when description is modified to set unsaved changes flag.
        /// </summary>
        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            _hasUnsavedChanges = true;
        }

        /// <summary>
        /// Handles the close button click.
        /// </summary>
        private void btnWindowClose_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the window closing event to check for unsaved changes.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_hasUnsavedChanges)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you want to discard them?",
                    "Unsaved Changes",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnCreateCategory_Clicked(object sender, RoutedEventArgs e)
        {
            _catPresenter.SaveCategory();
        }

        /// <summary>
        /// Handles the cancel button click.
        /// </summary>
        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            const string message = "Are you sure you would like to cancel?";
            const string caption = "Closing Window";

            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _hasUnsavedChanges = false; // Don't prompt again
                this.Close();
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
                _suppressCategoryCheckboxEvents = true; // Allows the user to uncheck a box without immediately re-checking it when the value is set to default
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
