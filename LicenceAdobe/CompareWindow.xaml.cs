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
        private List<ComputerRecord> records;
        private List<WebRecord> webRecords;
        public CompareWindow(List<ComputerRecord> localRecords, List<WebRecord> webOnlyRecords)
        {
            InitializeComponent();
            this.records = localRecords;
            this.webRecords = webOnlyRecords;

            showDiff();
        }

        private void showDiff()
        {
            object[] differences = findDifference(records, webRecords);            

            localRecordDataGrid.ItemsSource = (List<ComputerRecord>)differences[0];
            webOnlyDataGrid.ItemsSource = (List<WebRecord>)differences[1];
        }
        private static object[] findDifference(List<ComputerRecord> localRecords, List<WebRecord> webReportNames)
        {
            List<ComputerRecord> diffRecords = new List<ComputerRecord>();
            List<WebRecord> diffWebReportNames = new List<WebRecord>();
            foreach (ComputerRecord record in localRecords)
            {
                bool found = false;
                foreach (WebRecord webRecord in webReportNames)
                {
                    if (record.CName.ToUpper() == webRecord.Name.ToUpper())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    diffRecords.Add(record);
            }
            foreach (WebRecord webRecord in webReportNames)
            {
                bool found = false;
                foreach (ComputerRecord record in localRecords)
                {
                    if (record.CName.ToUpper() == webRecord.Name.ToUpper())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    diffWebReportNames.Add(webRecord);
            }
            return new Object[] { diffRecords, diffWebReportNames };
        }


        private void moveBtn_Click(object sender, RoutedEventArgs e)
        {
            recordsToAdd = new List<WebRecord>();

            recordsToAdd = webOnlyDataGrid.SelectedCells
                .Select(cell => (WebRecord) cell.Item)
                .Distinct()
                .ToList();

            webOnlyDataGrid.UnselectAll();
            localRecordDataGrid.UnselectAll();

            foreach (WebRecord webRecord in recordsToAdd)
            {
                ComputerRecord record = new ComputerRecord();
                record.CName = webRecord.Name;
                records.Add(record);
            }
            showDiff();
            //this.Hide();
        }

        private void compareDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            moveBtn.IsEnabled = (webOnlyDataGrid.SelectedCells.Count() > 0);

            int webSelected = webOnlyDataGrid.SelectedCells
                .Select(cell => (WebRecord)cell.Item)
                .Distinct().Count();

            int localSelected = (localRecordDataGrid.ItemsSource == null) 
                ? 0 
                :localRecordDataGrid.SelectedCells
                .Select(cell => (ComputerRecord)cell.Item)
                .Distinct().Count();

            updateBtn.IsEnabled = (localSelected == 1 && webSelected == 1);
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            WebRecord webSelectedItem = webOnlyDataGrid.SelectedCells
                .Select(cell => (WebRecord)cell.Item).First();

            ComputerRecord localSelected = localRecordDataGrid.SelectedCells
                .Select(cell => (ComputerRecord)cell.Item).First();

            localSelected.CName = webSelectedItem.Name;

            webOnlyDataGrid.UnselectAll();
            localRecordDataGrid.UnselectAll();

            //List<ComputerRecord> sourceTmp = (List<ComputerRecord>) localRecordDataGrid.ItemsSource;
            //localRecordDataGrid.ItemsSource = null;
            //localRecordDataGrid.ItemsSource = sourceTmp;

            showDiff();

        }
    }
}
