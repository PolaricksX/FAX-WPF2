using Calendar;
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
using System.Globalization;

namespace FAX_WPF
{
    /// <summary>
    /// Interaction logic for CreateEvents.xaml
    /// </summary>
    public partial class CreateEvents : Window, IEventView
    {
        private EventsPresenter _eventpresenter;
        public CreateEvents(MainPresenter p)
        {
            InitializeComponent();
            _eventpresenter = p.GetEventsPresenter(this);

            cmbCategoryName.DisplayMemberPath = nameof(Category.Description);

            var categories = _eventpresenter.GetCategories();
            foreach (var category in categories)
            {
                cmbCategoryName.Items.Add(category);
            }
        }

        private void cmbCategoryName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategoryName.SelectedItem is Category category)
            {
                txtCategoryID.Text = category.Id.ToString();
            }
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            // source: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.messageboxbuttons?view=windowsdesktop-10.0

            const string message = "Are you sure you would like to cancel?";
            const string caption = "Closing Window";

            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void btnCreateCategory_Clicked(object sender, RoutedEventArgs e)
        {
            CreateCategories createCategories = new CreateCategories();
            createCategories.Show();
            this.Close();
        }
        public int CategoryId
        {
            get
            {
                if (cmbCategoryName.SelectedItem is Category category)
                {
                    return category.Id;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                foreach (var item in cmbCategoryName.Items)
                {
                    if (item is Category category && category.Id == value)
                    {
                        cmbCategoryName.SelectedItem = item;
                        break;
                    }
                }
            }
        }


        public int DurationMinutes
        {
            get
            {
                if (int.TryParse(txtDuration.Text, out int duration))
                {
                    return duration;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                txtDuration.Text = value.ToString();
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                if (DateTime.TryParse(txtDateTime.Text, CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                {
                    return dt;
                }
                else
                {
                    return DateTime.Now;
                }
            }
            set
            {
                txtDateTime.Text = value.ToString();
            }
        }
        public string Details
        {
            get
            {
                if (txtDetails.Text != null)
                {
                    return txtDetails.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                txtDetails.Text = value;
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
