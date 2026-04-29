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

        public int CategoryId
        {
            get => int.TryParse(txtCategoryID.Text, out int value) ? value : 0;
            set => txtCategoryID.Text = value.ToString();
        }

        public int DurationMinutes
        {
            get => int.TryParse(txtDuration.Text, out int value) ? value : 0;
            set => txtDuration.Text = value.ToString();
        }

        public DateTime StartDateTime
        {
            get
            {
                const string format = "yyyy-MM-dd HH:mm";
                if (DateTime.TryParseExact(txtDateTime.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                {
                    return parsed;
                }

                return DateTime.TryParse(txtDateTime.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed)
                    ? parsed
                    : DateTime.MinValue;
            }
            set => txtDateTime.Text = value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        public string Details
        {
            get => txtDetails.Text;
            set => txtDetails.Text = value ?? string.Empty;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public CreateEvents(MainPresenter p)
        {
            InitializeComponent();
            _eventpresenter = p.GetEventsPresenter(this);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _eventpresenter.CreateEvent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
