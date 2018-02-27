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

namespace LicenceAdobe
{
    /// <summary>
    /// Interaction logic for CompareWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        public List<WebRecord> recordsToAdd = null;
        public CompareWindow(List<ComputerRecord> localRecords, List<WebRecord> webOnlyRecords)
        {
            InitializeComponent();
            localRecordDataGrid.ItemsSource = localRecords;
            webOnlyDataGrid.ItemsSource = webOnlyRecords;
        }

        private void moveAndCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            recordsToAdd = new List<WebRecord>();

            recordsToAdd = webOnlyDataGrid.SelectedCells
                .Select(cell => (WebRecord) cell.Item)
                .Distinct()
                .ToList();

            this.Hide();
        }

        private void compareDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            moveAndCloseBtn.IsEnabled = (webOnlyDataGrid.SelectedCells.Count() > 0);

            int webSelected = webOnlyDataGrid.SelectedCells
                .Select(cell => (WebRecord)cell.Item)
                .Distinct().Count();

            int localSelected = localRecordDataGrid.SelectedCells
                .Select(cell => (ComputerRecord)cell.Item)
                .Distinct().Count();
            moveAndCloseBtn.IsEnabled = (localSelected == 1 && webSelected == 1);
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
