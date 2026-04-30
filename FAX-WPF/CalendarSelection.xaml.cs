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
    /// Interaction logic for CalendarSelection.xaml
    /// </summary>
    public partial class CalendarSelections : Window
    {
        private readonly MainPresenter _mainPresenter;

        public CalendarSelections(MainPresenter mainPresenter)
        {
            InitializeComponent();
            _mainPresenter = mainPresenter;
        }

        private void BtnCreateCategory_Click(object sender, RoutedEventArgs e)
        {
            var createCategories = new CreateCategories(_mainPresenter);
            createCategories.ShowDialog();
        }

        private void BtnCreateEvent_Click(object sender, RoutedEventArgs e)
        {
            var createEvents = new CreateEvents(_mainPresenter);
            createEvents.ShowDialog();
        }
    }
}
