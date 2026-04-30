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
using System.Runtime.Versioning;

namespace FAX_WPF
{
    /// <summary>
    /// Main window for the FAX Calendar application.
    /// Provides UI for theme selection, calendar file management, and navigation.
    /// Implements IMainView interface for MVP pattern.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a new MainWindow with an existing calendar
    /// MainWindow mainWindow = new MainWindow("C:\\Users\\Documents\\Calendars\\Work.calendar", false);
    /// mainWindow.Show();
    /// 
    /// // Create a new MainWindow with a new database
    /// MainWindow newWindow = new MainWindow("C:\\Users\\Documents\\Calendars\\NewCal.calendar", true);
    /// newWindow.Show();
    /// ]]>
    /// </code>
    /// </example>
    public partial class MainWindow : Window, IMainView
    {
        private MainPresenter _mainPresenter;

        public MainWindow()
        {
            InitializeComponent();
            // If a previous file is stored in the registry, open it automatically and skip prompts
            string previousFile = null;
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\FAX_WPF");
                previousFile = key?.GetValue("LastUsedFile") as string;
            }
            catch
            {
                previousFile = null;
            }

            if (!string.IsNullOrEmpty(previousFile) && System.IO.File.Exists(previousFile))
            {
                _mainPresenter = new MainPresenter(this, previousFile, false);
                infoTextBlock.Text = $"Opened: {System.IO.Path.GetFileName(previousFile)}";
                return;
            }

            // Ask the user once whether they're a first-time user. If they are, prompt to create a calendar file.
            var firstTime = MessageBox.Show(
                "Is this your first time using the app?",
                "Welcome",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (firstTime == MessageBoxResult.Yes)
            {
                var savefile = new SaveFileDialog()
                {
                    Title = "Create calendar file",
                    Filter = "Calendar files (*.calendar)|*.calendar|All files (*.*)|*.*",
                    DefaultExt = ".calendar",
                    FileName = "MyCalendar"
                };

                if (savefile.ShowDialog() == true)
                {
                    string path = savefile.FileName;
                    try
                    {
                        System.IO.File.Create(path).Close();
                        Properties.Settings.Default.LastUsedDirectory = System.IO.Path.GetDirectoryName(path);
                        Properties.Settings.Default.Save();
                        SaveLastUsedFile(path);

                        _mainPresenter = new MainPresenter(this, path, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to create calendar file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _mainPresenter = new MainPresenter(this, null, false);
                    }
                }
                else
                {
                    _mainPresenter = new MainPresenter(this, null, false);
                }
            }
            else
            {
                // Not first time (or they chose No) — start app without creating a file
                _mainPresenter = new MainPresenter(this, null, false);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainWindow class with a specific calendar file.
        /// </summary>
        /// <param name="filename">Path to the calendar database file</param>
        /// <param name="newDB">True if creating a new database, false if opening existing</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Open an existing calendar file
        /// MainWindow window = new MainWindow("C:\\Users\\Documents\\Calendars\\Work.calendar", false);
        /// 
        /// // Create a new calendar database
        /// MainWindow newWindow = new MainWindow("C:\\Users\\Documents\\Calendars\\Personal.calendar", true);
        /// ]]>
        /// </code>
        /// </example>
        public MainWindow(string filename, bool newDB)
        {
            InitializeComponent();
            _mainPresenter = new MainPresenter(this, filename, newDB);
            ApplyTheme("Soft Blue");
        }

        private void ComboTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox combo)
            {
                return;
            }

            if (combo.SelectedItem is not ComboBoxItem selectedItem)
            {
                return;
            }

            ApplyTheme(selectedItem.Content?.ToString() ?? "Soft Blue");
        }

        /// <summary>
        /// Applies a color theme to the application by updating resource brushes.
        /// </summary>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.colorconverter.convertfromstring?view=windowsdesktop-10.0
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.solidcolorbrush?view=windowsdesktop-10.0
        /// </remarks>
        /// <param name="themeName">Name of the theme ("High Contrast" or "Soft Blue")</param>
        /// <example>
        /// 
        /// <code>
        /// <![CDATA[
        /// // Apply Soft Blue theme
        /// ApplyTheme("Soft Blue");
        /// 
        /// // Apply High Contrast theme
        /// ApplyTheme("High Contrast");
        /// 
        /// // Result: All brushes in Application.Current.Resources are updated
        /// var background = (SolidColorBrush)Application.Current.Resources["AppBackgroundBrush"];
        /// // background.Color will be #F7FAFC for Soft Blue or #121417 for High Contrast
        /// ]]>
        /// </code>
        /// </example>
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

        /// <summary>
        /// Updates an application resource brush with a new color.
        /// </summary>
        /// <param name="key">Resource dictionary key for the brush</param>
        /// <param name="hexColor">Hex color value (e.g., "#FFFFFF")</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Set background to white
        /// SetBrush("AppBackgroundBrush", "#FFFFFF");
        /// 
        /// // Set primary color to blue
        /// SetBrush("PrimaryBrush", "#2F6F8F");
        /// 
        /// // Later access in XAML
        /// // <Border Background="{DynamicResource AppBackgroundBrush}" />
        /// ]]>
        /// </code>
        /// </example>
        private static void SetBrush(string key, string hexColor)
        {
            var parsedColor = ColorConverter.ConvertFromString(hexColor);
            if (parsedColor is not Color color)
            {
                return;
            }

            Application.Current.Resources[key] = new SolidColorBrush(color);
        }

        /// <summary>
        /// Handles the Exit App button click event.
        /// Shuts down the entire application.
        /// </summary>
        /// <param name="sender">The button that triggered the event</param>
        /// <param name="e">Routed event arguments</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // In XAML:
        /// <Button Content="Exit App" Click="ExitApp_Click" />
        /// 
        /// // When clicked, application closes completely
        /// ]]>
        /// </code>
        /// </example>
        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Displays a message to the user in a message box.
        /// Implements IMainView.ShowMessage.
        /// </summary>
        /// <param name="message">Message text to display</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // Display a simple message
        /// ShowMessage("Calendar saved successfully!");
        /// 
        /// // Display an error message
        /// ShowMessage("Error: File not found");
        /// ]]>
        /// </code>
        /// </example>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Handles the Select Calendar button click event.
        /// Opens a file dialog to select an existing calendar file.
        /// Saves the selected file path to user settings and registry.
        /// </summary>
        /// <param name="sender">The button that triggered the event</param>
        /// <param name="e">Routed event arguments</param>
        /// <remarks>
        /// Uses the last used directory if available, otherwise defaults to Documents/Calendars.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // In XAML:
        /// <Button Content="Select Calendar" Click="SelectCalendar_Click"/>
        /// 
        /// // When clicked:
        /// // 1. Opens file dialog starting at last used directory (or Documents/Calendars)
        /// // 2. User selects "Work.calendar"
        /// // 3. Path saved to Properties.Settings.Default.LastUsedDirectory
        /// // 4. Path saved to Registry: HKCU\SOFTWARE\FAX_WPF\LastUsedFile
        /// // 5. tbInfo displays: "Opened: Work.calendar"
        /// ]]>
        /// </code>
        /// </example>
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-10.0
        private void SelectCalendar_Click(object sender, RoutedEventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string defaultFolder = System.IO.Path.Combine(docPath, "Calendars");

            // use last directory if it exists, otherwise use default
            // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-create-a-new-settingat-design-time
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

                // save the directory for next time
                // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-write-usersettings-at-run-time-with-csharp
                Properties.Settings.Default.LastUsedDirectory = System.IO.Path.GetDirectoryName(selectedPath);
                Properties.Settings.Default.Save();

                infoTextBlock.Text = $"Opened: {System.IO.Path.GetFileName(selectedPath)}";

                SaveLastUsedFile(selectedPath);
                _mainPresenter = new MainPresenter(this, selectedPath, false);
                var selections = new CalendarSelections(_mainPresenter);
                selections.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Save Calendar button click event.
        /// Opens a file dialog to create a new calendar file.
        /// Creates the file and saves its path to the registry.
        /// </summary>
        /// <param name="sender">The button that triggered the event</param>
        /// <param name="e">Routed event arguments</param>
        /// <remarks>
        /// Creates the Calendars folder in Documents if it doesn't exist.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.io.file.create
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // In XAML:
        /// <Button Content="Save Calendar" Click="SaveCalendar_Click"/>
        /// 
        /// // When clicked:
        /// // 1. Opens save dialog with default name "MyCalendar.calendar"
        /// // 2. User changes name to "WorkCalendar.calendar"
        /// // 3. Creates empty file at C:\Users\<username>\Documents\Calendars\WorkCalendar.calendar
        /// // 4. Saves path to Registry
        /// // 5. tbInfo displays: "Created: C:\Users\<username>\Documents\Calendars\WorkCalendar.calendar"
        /// ]]>
        /// </code>
        /// </example>
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
                string finalPath = saveFileDialog.FileName;

                //https://learn.microsoft.com/en-us/dotnet/api/system.io.file.create?view=net-10.0 creating file
                File.Create(finalPath).Close();

                infoTextBlock.Text = $"Created: {System.IO.Path.GetFullPath(finalPath)}";

                SaveLastUsedFile(finalPath);
            }
        }

        [SupportedOSPlatform("windows")]
        private static void SaveLastUsedFile(string filePath)
        {
            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.registry?view=net-10.0
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\FAX_WPF");
            key.SetValue("LastUsedFile", filePath);
        }

        [SupportedOSPlatform("windows")]
        private void PreviousFile_Click(object sender, RoutedEventArgs e)
        {
            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.registry.currentuser?view=net-10.0#microsoft-win32-registry-currentuser
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\FAX_WPF");

            string lastusedFile = key?.GetValue("LastUsedFile") as string;

            if (string.IsNullOrEmpty(lastusedFile) || !File.Exists(lastusedFile))
            {
                MessageBox.Show("No previous file found or the file does not exist");
                return;
            }

            infoTextBlock.Text = $"Opened: {System.IO.Path.GetFileName(lastusedFile)}";
            _mainPresenter = new MainPresenter(this, lastusedFile, false);

            var selections = new CalendarSelections(_mainPresenter)
            {
                Owner = this
            };

            selections.ShowDialog();
        }
    }
}