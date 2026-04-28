using Microsoft.Win32;
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
using System.IO;

namespace FAX_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private MainPresenter _mainpresenter;

        public MainWindow()
        {
            InitializeComponent();
        }
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

        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-10.0
        private void SelectCalendar_Click(object sender, RoutedEventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string defaultFolder = System.IO.Path.Combine(docPath, "Calendars");

            // use last directory if it exists, otherwise use default
            // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-create-a-new-setting-at-design-time
            string initialDirectory = Properties.Settings.Default.LastUsedDirectory;
            if (string.IsNullOrEmpty(initialDirectory) || !Directory.Exists(initialDirectory))
            {
                initialDirectory = defaultFolder;
                if (!Directory.Exists(initialDirectory))
                {
                    Directory.CreateDirectory(initialDirectory);
                }
            }


            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select a calendar file",
                InitialDirectory = initialDirectory,
                Filter = "Calendar files (*.calendar)|*.calendar|All files (*.*)|*.*",
                DefaultExt = ".calendar"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName;

                // take that selectedPath and give it to the presenter??

                // save the directory for next time
                // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-write-user-settings-at-run-time-with-csharp
                Properties.Settings.Default.LastUsedDirectory = System.IO.Path.GetDirectoryName(selectedPath);
                Properties.Settings.Default.Save();

                tbInfo.Text = $"Opened: {System.IO.Path.GetFileName(selectedPath)}";
            }
        }
        

        private void SaveCalendar_Click(object sender, RoutedEventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);


            string defaultFolder = System.IO.Path.Combine(docPath, "Calendars");


            // making sure the subfolder actually exists
            if (!System.IO.Directory.Exists(defaultFolder))
            {
                System.IO.Directory.CreateDirectory(defaultFolder);
            }


            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Name your new calendar file",
                InitialDirectory = defaultFolder,
                DefaultExt = ".calendar",
                FileName = "MyCalendar"
            };


            if (saveFileDialog.ShowDialog() == true)
            {
                // this is the full path the user chose (e.g., .../Documents/Calendars/Work.db)
                string finalPath = saveFileDialog.FileName;

                //https://learn.microsoft.com/en-us/dotnet/api/system.io.file.create?view=net-10.0 creating file
                File.Create(finalPath).Close();


                tbInfo.Text = $"Created: {System.IO.Path.GetFullPath(finalPath)}";


                //HomeCalendar.SaveToFile(finalPath);
            }
        }
    }
    
}