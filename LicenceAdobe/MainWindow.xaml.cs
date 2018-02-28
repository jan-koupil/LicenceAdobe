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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Text.RegularExpressions;



namespace LicenceAdobe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ComputerRecord> records = new List<ComputerRecord>();
        private List<WebRecord> webReportNames = new List<WebRecord>();
        public MainWindow()
        {
            InitializeComponent();
            recordDataGrid.ItemsSource = records;
            webReportNamesDataGrid.ItemsSource = webReportNames;
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml"; // Default file extension
            openFileDialog.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension            
            if (openFileDialog.ShowDialog() == true)
                try
                {
                    records = XMLSerialization.ReadFromXmlFile<List<ComputerRecord>>(openFileDialog.FileName);
                }
                catch
                {
                    string message = "Nelze načíst soubor";
                    string caption = "Chyba";
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);
                }
            showData();
        }


        private void showData()
        {
            recordDataGrid.ItemsSource = null;
            recordDataGrid.ItemsSource = records;
            webReportNamesDataGrid.ItemsSource = null;
            webReportNamesDataGrid.ItemsSource = webReportNames;
            
            regCountTb.Text = records.Count().ToString();
            webCountTb.Text = webReportNames.Count().ToString();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime thisDay = DateTime.Today;
            string defaultFilename = String.Format("Licence_{0}_{1:00}_{2:00}", thisDay.Year, thisDay.Month, thisDay.Day);
            try
            {                
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = defaultFilename;
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    XMLSerialization.WriteToXmlFile(dlg.FileName, records);
                }
            }
            catch
            {
                string message = "Chyba zápisu do souboru";
                string caption = "Chyba";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            showData();
        }

        private void importCSVDialogOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".csv"; // Default file extension
            openFileDialog.Filter = "Comma separated values (.csv)|*.csv"; // Filter files by extension            
            if (openFileDialog.ShowDialog() == true)
                try
                {
                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.  
                    System.IO.StreamReader file =
                        new System.IO.StreamReader(openFileDialog.FileName);

                    webReportNames.Clear();

                    while ((line = file.ReadLine()) != null)
                    {
                        if (counter != 0)
                        {
                            WebRecord parsed = parseCSVLine(line);
                            if (parsed != null) webReportNames.Add(parsed);
                        }
                        counter++;
                    }

                    file.Close();
                }
                catch
                {
                    string message = "Nelze načíst soubor";
                    string caption = "Chyba";
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);
                }
            showData();
        }

        private WebRecord parseCSVLine(string csv)
        {
            string[] values = csv.Split(',');
            WebRecord record = new WebRecord();
            try
            {
                record.Name = values[0].Trim();
                record.Product = values[1].Trim();
                record.Status = values[2].Trim();
                record.Expiration = values[3].Trim();
            }
            catch {
                if (record.Name == "")
                    return null;
            }
            return record;            
        }

        private void compareBtn_Click(object sender, RoutedEventArgs e)
        {
            object[] differences = findDifference();
            CompareWindow compWin = new CompareWindow((List<ComputerRecord>) differences[0], (List<WebRecord>)differences[1]);
            compWin.ShowDialog();
            List<WebRecord> recordsToAdd = compWin.recordsToAdd;
            compWin.Close();
            if (recordsToAdd == null)
            {
                showData();
                return;
            }

            foreach (WebRecord webRecord in recordsToAdd)
            {
                ComputerRecord record = new ComputerRecord();
                record.CName = webRecord.Name;
                records.Add(record);
            }
            showData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                try
                {
                    App.Current.Windows[intCounter].Close();
                }
                catch { };
        }

        private object[] findDifference()
        {
            List<ComputerRecord> diffRecords = new List<ComputerRecord>();
            List<WebRecord> diffWebReportNames = new List<WebRecord>();
            foreach (ComputerRecord record in records)
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
                foreach (ComputerRecord record in records)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}
