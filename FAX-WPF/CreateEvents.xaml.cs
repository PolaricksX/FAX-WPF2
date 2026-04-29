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
                if (DateTime.TryParse(txtDateTime.Text, out DateTime dt))
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

        private void txtCategoryID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtDuration_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtDateTime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtDetails_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
