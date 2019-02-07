using CsvHelper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Controls;

namespace Dashboard
{

    // Class for each motor in the logger.
    public class MotorEntry
    {
        public string MotorName { get; set; }
        public bool LogInfo { get; set; }
        public double MotorCurrent { get; set; }
        public double MotorVoltage { get; set; }
        public double MotorRPM { get; set; }
        public double MotorPosition { get; set; }
    }

    /// <summary>
    /// Interaction logic for Logger.xaml
    /// </summary>
    public partial class Logger : UserControl
    {

        // Collection of motor entries for the data grid.
        ObservableCollection<MotorEntry> MotorEntries = new ObservableCollection<MotorEntry>();

        // Variable to track the logging state.
        private bool LoggingActive = false;

        // Background worker which will save the data to the CSV file
        private BackgroundWorker UpdateCSV = new BackgroundWorker();

        // Writer to write to CSV file
        StreamWriter CSVStreamWriter;
        CsvWriter CSVFileWriter;

        public Logger()
        {
            InitializeComponent();

            // Binding the data grid to the motor entries list.
            LoggerDataGrid.ItemsSource = MotorEntries;

            MotorEntries.Add(new MotorEntry() { MotorName = "liftMaster", LogInfo = true });
            MotorEntries.Add(new MotorEntry() { MotorName = "liftSlavePrimary", LogInfo = true });
            MotorEntries.Add(new MotorEntry() { MotorName = "liftSlaveSecondary", LogInfo = true });
            MotorEntries.Add(new MotorEntry() { MotorName = "liftSlaveTertiary", LogInfo = true });

            // Link the log data method to the background worker
            UpdateCSV.DoWork += LogDataFromDataGrid;

            // Have the update csv run in its own thread
            UpdateCSV.RunWorkerAsync();

        }

        private void LoggerDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            // Change some column information based on the column type.
            switch (e.Column.Header)
            {

                case "MotorName":
                    e.Column.Header = "Motor Name";
                    e.Column.IsReadOnly = true;
                    break;

                case "LogInfo":
                    e.Column.Header = "Log Info?";
                    break;

                case "MotorVoltage":
                    e.Column.Header = "Voltage";
                    break;

                case "MotorCurrent":
                    e.Column.Header = "Current";
                    break;

                case "MotorRPM":
                    e.Column.Header = "RPM";
                    break;

                default:
                    break;

            }
        }

        // Function that returns the current logging state.
        public bool IsLogging()
        {
            return LoggingActive;
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoggingActive = true;

            // Initialize CSV stream writer and CSV file writer with current date and time
            CSVStreamWriter = new StreamWriter("C:\\Users\\Public\\Documents\\log_" + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + ".csv");
            CSVFileWriter = new CsvWriter(CSVStreamWriter, true);
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoggingActive = false;

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;

            // Close CSV stream writer
            CSVStreamWriter.Close();
        }

        public void SetDataInDataGrid(string Name, string Aspect, double Value)
        {
            foreach (MotorEntry Motor in MotorEntries)
            {
                if (Name == Motor.MotorName)
                {
                    switch (Aspect)
                    {

                        case "Current":
                            Motor.MotorCurrent = Value;
                            break;

                        case "Voltage":
                            Motor.MotorVoltage = Value;
                            break;

                        case "EncoderVelocity":
                            Motor.MotorRPM = Value;
                            break;

                        case "EncoderPosition":
                            Motor.MotorPosition = Value;
                            break;

                        default:
                            break;

                    }
                }
            }

            // Refresh the logger's datagrid.
            LoggerDataGrid.Items.Refresh();

        }

        public void LogDataFromDataGrid(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                // Record the start time.
                TimeSpan StartTime = DateTime.UtcNow - new DateTime(1970, 1, 1);

                if (IsLogging())
                {
                    // Log data to the CSV file
                    CSVFileWriter.WriteRecords(MotorEntries);
                    CSVFileWriter.Flush();
                }

                // Record the end time.
                TimeSpan StopTime = DateTime.UtcNow - new DateTime(1970, 1, 1);

                // Sleep the alloted time.
                Thread.Sleep(50 - ((int)StopTime.TotalMilliseconds - (int)StartTime.TotalMilliseconds));
            }
        }
    }
}
