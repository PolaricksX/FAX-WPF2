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
        private CategoryType _selectedCategoryType;
        public CreateCategories()
        {
            InitializeComponent();
        }

        private void btnCreateCategory_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
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

        public CategoryType SelectedCategoryType
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
                    case CategoryType.Event:
                        cbEvent.IsChecked = true;
                        break;
                    case CategoryType.AllDayEvent:
                        cbAllDayEvent.IsChecked = true; 
                        break;
                    case CategoryType.Holiday:
                        cbHoliday.IsChecked = true;
                        break;
                    case CategoryType.Availability:
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
                    SelectedCategoryType = CategoryType.Event;
                    break;
                case "cbAllDayEvent":
                    SelectedCategoryType = CategoryType.AllDayEvent;
                    break;
                case "cbHoliday":
                    SelectedCategoryType = CategoryType.Holiday;
                    break;
                case "cbAvailability":
                    SelectedCategoryType = CategoryType.Availability;
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
