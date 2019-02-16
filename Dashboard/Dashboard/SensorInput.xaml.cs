using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Dashboard
{
    // Class for the encoders and hatchersnatcher.
    public class SensorEntry
    {
        public string EncoderName { get; set; }
        public double EncoderValue { get; set; }

    }

    /// <summary>
    /// Interaction logic for SensorInput.xaml
    /// </summary>
    public partial class SensorInput : UserControl
    {

        // Collection of encoders and hatchersnatcher for the data grid.
        ObservableCollection<SensorEntry> SensorEntries = new ObservableCollection<SensorEntry>();

        public SensorInput()
        {
            InitializeComponent();

            // Binding data to encoder entries list.
            SensorInputGrid.ItemsSource = SensorEntries;

            SensorEntries.Add(new SensorEntry { EncoderName = "leftDriveEncoderOne", EncoderValue = 0});
            SensorEntries.Add(new SensorEntry { EncoderName = "leftDriveEncoderTwo", EncoderValue = 0 });
            SensorEntries.Add(new SensorEntry { EncoderName = "leftDriveEncoderThree", EncoderValue = 0 });
            SensorEntries.Add(new SensorEntry { EncoderName = "rightDriveEncoderOne", EncoderValue = 0 });
            SensorEntries.Add(new SensorEntry { EncoderName = "rightDriveEncoderTwo", EncoderValue = 0 });
            SensorEntries.Add(new SensorEntry { EncoderName = "rightDriveEncoderThree", EncoderValue = 0 });
            SensorEntries.Add(new SensorEntry { EncoderName = "fourBarEncoder", EncoderValue = 0 });
            SensorEntries.Add(new SensorEntry { EncoderName = "liftEncoder", EncoderValue = 0 });

        }

        private void SensorInputGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
