using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FAX_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private MainPresenter _presenter;
        public MainWindow()
        {
            InitializeComponent();
            _presenter = new MainPresenter(this);
        }

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            CreateEvents createEvent = new CreateEvents(_presenter);
        }

        private void CreateCategory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}