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
        private MainPresenter _mainpresenter;
        public MainWindow(string filename, bool newDB)
        {
            InitializeComponent();
            _mainpresenter = new(this, filename, newDB);
            ApplyTheme("Soft Blue");
        }

        

        private void cmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   if (sender is not ComboBox cmb)
            {
                return;
            }

            if (cmb.SelectedItem is not ComboBoxItem selectedItem)
            {
                return;
            }

            ApplyTheme(selectedItem.Content?.ToString() ?? "Soft Blue");
        }

        private static void ApplyTheme(string themeName)
        {
            if (themeName == "High Contrast")
            {
                SetBrush("AppBackgroundBrush", "#121417");
                SetBrush("SurfaceBrush", "#1E2329");
                SetBrush("PrimaryBrush", "#F4B400");
                SetBrush("AccentBrush", "#00C2A8");
                SetBrush("TextBrush", "#F4F6F8");
                SetBrush("DangerBrush", "#FF5A5F");
                SetBrush("ControlBorderBrush", "#5B6470");
                SetBrush("InputBackgroundBrush", "#2A3138");
                return;
            }

            SetBrush("AppBackgroundBrush", "#F7FAFC");
            SetBrush("SurfaceBrush", "#FFFFFF");
            SetBrush("PrimaryBrush", "#2F6F8F");
            SetBrush("AccentBrush", "#4F7C6A");
            SetBrush("TextBrush", "#1F2933");
            SetBrush("DangerBrush", "#C0392B");
            SetBrush("ControlBorderBrush", "#AAB6C3");
            SetBrush("InputBackgroundBrush", "#FFFFFF");
        }

        private static void SetBrush(string key, string hexColor)
        {
            var parsedColor = ColorConverter.ConvertFromString(hexColor);
            if (parsedColor is not Color color)
            {
                return;
            }

            Application.Current.Resources[key] = new SolidColorBrush(color);
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}