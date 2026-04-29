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
    }
}
