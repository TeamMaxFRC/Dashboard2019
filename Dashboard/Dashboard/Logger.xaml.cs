using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dashboard
{

    /// <summary>
    /// Interaction logic for Logger.xaml
    /// </summary>
    public partial class Logger : UserControl
    {

        // Background worker that is remove any stale log files.
        private BackgroundWorker StaleLogChecker = new BackgroundWorker();

        // Log tracking class.
        public class Log
        {
            public string BundleIdentifier { get; set; }
            public string ActiveFileName { get; set; }
            public DateTime LastTimeLogged { get; set; }
        }

        // List of log files open.
        List<Log> ActiveLogs = new List<Log>();
        
        // Constructor for the logging widget.
        public Logger()
        {
            InitializeComponent();

            // Start the stale log thread.
            StaleLogChecker.DoWork += RemoveStaleLogs;
            StaleLogChecker.RunWorkerAsync();
        }

        // Log the data provided.
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void LogData(string BundleIdentifier, string HeaderLine, string DataLine)
        {

            // Create the specific directory if it doesn't exist.
            System.IO.Directory.CreateDirectory("C:/Max Dashboard Log Files/");

            // Boolean to check if the file must be initialized.
            bool InitializeFile = true;

            // String to track the current file name.
            string FileName = "";

            // Check if the bundle identifier exists in the list.
            foreach (Log LogFile in ActiveLogs)
            {
                if (LogFile.BundleIdentifier == BundleIdentifier)
                {
                    FileName = LogFile.ActiveFileName;
                    LogFile.LastTimeLogged = DateTime.Now;
                    InitializeFile = false;
                    break;
                }
            }

            // Initialize the file, if it doesn't exist yet.
            if (InitializeFile)
            {

                // Create the new log file and add it to the active files.
                Log NewLogFile = new Log() { BundleIdentifier = BundleIdentifier, ActiveFileName = BundleIdentifier + "-" + DateTime.Now.ToString("hh-mm-ss"), LastTimeLogged = DateTime.Now };
                ActiveLogs.Add(NewLogFile);

                // Store the new file name.
                FileName = NewLogFile.ActiveFileName;

                // Create a stream writer to append the header.
                StreamWriter HeaderWriter = File.AppendText("C:/Max Dashboard Log Files/" + FileName + ".csv");

                // Write the header to the file and close the stream writer.
                HeaderWriter.WriteLine(HeaderLine);
                HeaderWriter.Flush();
                HeaderWriter.Close();
                                
            }

            // Create the stream writer for the CSV file.
            StreamWriter DataWriter = File.AppendText("C:/Max Dashboard Log Files/" + FileName + ".csv");

            // Write the line to the file and close the stream writer.
            DataWriter.WriteLine(DataLine);
            DataWriter.Flush();
            DataWriter.Close();

        }

        // Removes any logs that haven't been updated for 5 seconds.
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ProcessRemoveStaleLogs()
        {
            // Check if any of the logs should be closed.
            foreach (Log LogFile in ActiveLogs)
            {
                if (LogFile.LastTimeLogged.AddSeconds(5) < DateTime.Now)
                {
                    ActiveLogs.Remove(LogFile);
                    break;
                }
            }
        }

        // Removes any logs that haven't been updated for 5 seconds.
        public void RemoveStaleLogs(object sender, DoWorkEventArgs e)
        {

            while (true)
            {

                // Record the start time.
                DateTime StartTime = DateTime.Now;

                ProcessRemoveStaleLogs();

                // Record the stop time.
                DateTime StopTime = DateTime.Now;

                // Sleep the background worker for 1 second
                TimeSpan PassedTime = StopTime - StartTime;
                Thread.Sleep(1000);

            }

        }

    }

}
