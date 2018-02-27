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
    /// Interaction logic for CsvImport.xaml
    /// </summary>
    public partial class CsvImport : Window
    {
        public string csvText = String.Empty;
        public CsvImport()
        {
            InitializeComponent();
        }

        private void closeCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            csvText = csvData.Text;
            this.Close();
        }
    }
}
