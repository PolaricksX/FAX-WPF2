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
            _eventpresenter = p.createEventsPresenter(this);

        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
