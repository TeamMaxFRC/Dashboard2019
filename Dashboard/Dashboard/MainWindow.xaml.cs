using SharpOSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ControllerData
    {
        public List<double> AxisList { get; set; }
        public List<bool> ButtonList { get; set; }
        public bool DPadUp { get; internal set; }
        public bool DPadDown { get; internal set; }
        public bool AButton { get; internal set; }
        public bool BButton { get; internal set; }
        public bool XButton { get; internal set; }
        public bool YButton { get; internal set; }
        public bool DPadLeft { get; internal set; }
        public bool DPadRight { get; internal set; }
        public bool MenuButton { get; internal set; }
        public bool ViewButton { get; internal set; }
        public bool XboxButton { get; internal set; }
        public bool LeftBumper { get; internal set; }
        public bool RightBumper { get; internal set; }
        public bool AButton2 { get; internal set; }
        public bool BButton2 { get; internal set; }
        public bool XButton2 { get; internal set; }
        public bool YButton2 { get; internal set; }
        public bool DPadUp2 { get; internal set; }
        public bool DPadDown2 { get; internal set; }
        public bool DPadLeft2 { get; internal set; }
        public bool DPadRight2 { get; internal set; }
        public bool MenuButton2 { get; internal set; }
        public bool XboxButton2 { get; internal set; }
        public bool LeftBumper2 { get; internal set; }
        public bool RightBumper2 { get; internal set; }

        public ControllerData()
        {
            AxisList = new List<double>
            {
                0.0, 0.0, 0.0, 0.0, 0.0, 0.0
            };
            ButtonList = new List<bool>
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false
            };
        }
    }

    public partial class MainWindow : Window
    {
        public bool Snapped = false, SnappedSearch = true;
        public bool DriverStationPresent = false;
        public double ScreenConstant = SystemParameters.WorkArea.Height / System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

        // Background worker which will receive the OSC data.
        private BackgroundWorker DriverStationWorker = new BackgroundWorker();
        private BackgroundWorker StreamDeckWorker = new BackgroundWorker();
        private BackgroundWorker LimelightWorker = new BackgroundWorker();
        private BackgroundWorker UpdateWorker = new BackgroundWorker();

        // Create the OSC receiver.
        private static UDPListener Receiver;

        public MainWindow()
        {
            InitializeComponent();
            LoadDriverStation();

            // Start the stream deck control.
            ManageStreamDeckProcesses(true);

            // Connect the receiver to the proper port.
            Receiver = new UDPListener(5803);

            // Link the update method to the background worker.
            DriverStationWorker.DoWork += DriverStationUpdate;
            StreamDeckWorker.DoWork += StreamDeckUpdate;
            LimelightWorker.DoWork += LimelightUpdate;
            UpdateWorker.DoWork += Update;

            // Have the update worker run in its own thread.
            DriverStationWorker.RunWorkerAsync();
            StreamDeckWorker.RunWorkerAsync();
            LimelightWorker.RunWorkerAsync();
            UpdateWorker.RunWorkerAsync();
        }

        // Loads the FRC driver station.
        public void LoadDriverStation()
        {

            try
            {
                if (!ManageDriverStation(false))
                {
                    Process.Start(@"C:\Program Files (x86)\FRC Driver Station\DriverStation.exe");
                }
            }
            catch
            {

            }

            // Find the driver station.
            DriverStationPresent = ManageDriverStation(true);
        }

        // Find the location of the driver station.
        public bool ManageDriverStation(bool Resize)
        {
            try
            {
                List<IntPtr> PossibleWindows = FindWindowsWithText("FRC Driver Station - ").ToList();

                if (PossibleWindows.Count != 0)
                {

                    Rect Rectangle = new Rect();

                    foreach (IntPtr Window in PossibleWindows)
                    {
                        GetWindowRect(Window, ref Rectangle);

                        if (Rectangle.Left == 0 && Rectangle.Right >= SystemParameters.PrimaryScreenWidth - 1 && Rectangle.Top > SystemParameters.PrimaryScreenHeight * .5)
                        {
                            double Height = ScreenConstant * Rectangle.Top;
                            if (Resize && SnappedSearch && (Application.Current.MainWindow.Height != Height || Application.Current.MainWindow.Width != SystemParameters.PrimaryScreenWidth))
                            {
                                SetSnappedMode(true);
                                MainDashboard.Left = 0;
                                MainDashboard.Top = 0;
                                MainDashboard.Width = SystemParameters.PrimaryScreenWidth;
                                Application.Current.MainWindow.Height = Height;
                                SetForegroundWindow(Window);
                            }
                            return true;
                        }

                    }

                    SetSnappedMode(false);
                    SnappedSearch = true;
                    return false;

                }
                else
                {
                    SetSnappedMode(false);
                    SnappedSearch = true;
                    return false;
                }

            }
            catch
            {
                SetSnappedMode(false);
                SnappedSearch = true;
                return false;
            }
        }

        public void SetSnappedMode(bool Set)
        {
            if (!Snapped && Set) // Snapped mode.
            {
                MainDashboard.ResizeMode = ResizeMode.CanMinimize;
                TitleBar.Height = 24;
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            }
            else if (Snapped && !Set) // Normal mode.
            {
                MainDashboard.ResizeMode = ResizeMode.CanResize;
                TitleBar.Height = 0;
                Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            Snapped = Set;
        }

        public void DriverStationUpdate(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.Current.Dispatcher.InvokeAsync(new Action(() => DriverStationPresent = ManageDriverStation(true)));
                Thread.Sleep(5000);
            }
        }

        public void StreamDeckUpdate(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(2000);
                ManageStreamDeckProcesses(false);
            }
        }

        public void LimelightUpdate(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                LimelightWidget.CheckStream();
                Thread.Sleep(2000);
            }
        }

        public void Update(object sender, DoWorkEventArgs e)
        {
            // Receive loop from Sharp OSC.
            while (true)
            {

                try
                {

                    // If the receiver exists, then receive the packet. Skip if it's null.
                    if (Receiver == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    OscPacket Packet = Receiver.Receive();

                    if (Packet == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    if (Packet.GetType().Name == "OscMessage")
                    {

                        // Get the next message. This receive will not block.
                        OscMessage ReceivedMessage = (OscMessage)Packet;

                        // Show any received Limelight values.
                        if (ReceivedMessage.Address.Equals("/Robot/Limelight/X"))
                        {
                            Application.Current.Dispatcher.InvokeAsync(new Action(() => LimelightWidget.UpdateX((double)ReceivedMessage.Arguments[0])));
                        }
                        if (ReceivedMessage.Address.Equals("/Robot/Limelight/Y"))
                        {
                            Application.Current.Dispatcher.InvokeAsync(new Action(() => LimelightWidget.UpdateY((double)ReceivedMessage.Arguments[0])));
                        }
                        if (ReceivedMessage.Address.Equals("/Robot/Limelight/A"))
                        {
                            Application.Current.Dispatcher.InvokeAsync(new Action(() => LimelightWidget.UpdateA((double)ReceivedMessage.Arguments[0])));
                        }

                        // Show any received Console values.
                        if (ReceivedMessage.Address.Equals("/Robot/Console/Text"))
                        {
                            Application.Current.Dispatcher.InvokeAsync(new Action(() => ConsoleBox.PrintLine((string)ReceivedMessage.Arguments[0])));
                        }

                    }
                    else
                    {

                        // The packet is actually a bundle, and should be logged.
                        OscBundle Bundle = (OscBundle)Packet;

                        // Log the bundle data if the bundle ID contains the word log.
                        if (((string)Bundle.Messages[0].Arguments[0]).Contains("Log"))
                        {

                            // Create the relevant data for the CSV file.
                            string BundleIdentifier = "";
                            string HeaderLine = "";
                            string DataLine = "";

                            // Iterate through all the messages and generate the header and data rows.
                            foreach (OscMessage Message in Bundle.Messages)
                            {

                                if (Message.Address.Equals("/BundleIdentifier"))
                                {
                                    BundleIdentifier = (string)Message.Arguments[0];
                                    continue;
                                }

                                HeaderLine += Message.Address + ",";
                                DataLine += ((double)Message.Arguments[0]).ToString() + ",";

                            }

                            // Call the log data function.
                            Application.Current.Dispatcher.InvokeAsync(new Action(() => LoggerWidget.LogData(BundleIdentifier, HeaderLine, DataLine)));

                        }

                        // If the bundle ID is "CurrentBundle" then send the data to the current widget.
                        if (((string)Bundle.Messages[0].Arguments[0]).Equals("CurrentBundle"))
                        {

                            // Iterate through all the messages and generate the header and data rows.
                            foreach (OscMessage Message in Bundle.Messages)
                            {

                                if (!Message.Address.Equals("/BundleIdentifier"))
                                {
                                    Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetCurrentMeter((double)Message.Arguments[0], Message.Address)));
                                }

                            }

                        }

                        // If the bundle ID is "SensorInputBundle" then send the data to the current widget.
                        if (((string)Bundle.Messages[0].Arguments[0]).Equals("SensorInputBundle"))
                        {

                            // Iterate through all the messages and generate the header and data rows.
                            foreach (OscMessage Message in Bundle.Messages)
                            {

                                if (!Message.Address.Equals("/BundleIdentifier"))
                                {
                                    Application.Current.Dispatcher.InvokeAsync(new Action(() => SensorInputWidget.SetSensorValue((double)Message.Arguments[0], Message.Address)));
                                }

                            }

                        }

                        // If the bundle ID is "ErrorBundle" then send the data to the error reporter.
                        if (((string)Bundle.Messages[0].Arguments[0]).Equals("ErrorBundle"))
                        {

                            List<string> Errors = new List<string>();

                            // Iterate through all the messages and generate the header and data rows.
                            foreach (OscMessage Message in Bundle.Messages)
                            {
                                if (!Message.Address.Equals("/BundleIdentifier"))
                                {

                                    foreach (int Argument in Message.Arguments)
                                    {
                                        if (Argument != -1)
                                        {
                                            Errors.Add(ConvertErrorAddress(Message.Address) + ConvertFault(Argument));
                                        }
                                    }
                                }
                            }

                            Application.Current.Dispatcher.InvokeAsync(new Action(() => ErrorWidget.SetErrors(Errors)));

                        }

                        if (((string)Bundle.Messages[0].Arguments[0]).Equals("ControllerDataBundle"))
                        {

                            // Iterate through all the messages and save controller data.
                            foreach (OscMessage Message in Bundle.Messages)
                            {

                                switch (Message.Address)
                                {

                                    case "/BundleIdentifier":
                                        break;

                                    case "/DriverController/Buttons":

                                        foreach (int ButtonState in Message.Arguments)
                                        {

                                        }
                                        break;

                                    case "/DriverController/Axis":
                                        foreach (double AxisValue in Message.Arguments)
                                        {

                                        }
                                        break;

                                    case "/OperatorController/Buttons":
                                        foreach (int ButtonState in Message.Arguments)
                                        {

                                        }
                                        break;

                                    default:
                                        break;

                                }
                            }

                        }



                    }

                }
                catch (Exception Ex)
                {
                    // Catch any exceptions.
                    MessageBox.Show(Ex.Message + Ex.StackTrace, "OSC Receive Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                }

            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Stop the program when the exit button is clicked.
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimize the program when the minimize button is clicked.
        }

        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!Snapped)
            {
                DragMove(); // Let the user move the window if not in snapped mode.
            }
        }

        // Convert the fault ID into a human readable string.
        private string ConvertFault(int Id)
        {

            switch (Id)
            {

                case 0:
                    return "Brownout";

                case 1:
                    return "Over Current";

                case 2:
                    return "Over Voltage";

                case 3:
                    return "Motor Fault";

                case 4:
                    return "Sensor Fault";

                case 5:
                    return "Stall";

                case 6:
                    return "EEPROMCRC";

                case 7:
                    return "CANTX";

                case 8:
                    return "CANRX";

                case 9:
                    return "Has Reset";

                case 10:
                    return "DRV Fault";

                case 11:
                    return "Other Fault!";

                case 12:
                    return "Soft Limit Fwd";

                case 13:
                    return "Soft Limit Rev";

                case 14:
                    return "Hard Limit Fwd";

                case 15:
                    return "Hard Limit Rev";

                default:
                    return "";

            }

        }

        // Convert the fault ID into a human readable string.
        private string ConvertErrorAddress(string Address)
        {

            switch (Address)
            {

                case "/LeftMasterFaults":
                    return "Left Master Spark: ";

                case "/RightMasterFaults":
                    return "Right Master Spark: ";

                case "/LeftSlavePrimaryFaults":
                    return "Left Slave Primary Spark: ";

                case "/RightSlavePrimaryFaults":
                    return "Right Slave Primary Spark: ";

                case "/LeftSlaveSecondaryFaults":
                    return "Left Slave Secondary Spark: ";

                case "/RightSlaveSecondaryFaults":
                    return "Right Slave Secondary Spark: ";

                default:
                    return "";

            }

        }

        private void MainDashboard_Closing(object sender, CancelEventArgs e)
        {
            // Kill all the threads to prevent the program from crashing.
            StreamDeckWorker.Dispose();
            LimelightWorker.Dispose();
            UpdateWorker.Dispose();

            // Close the OSC receiver.
            Receiver.Close();
        }

        public void ManageStreamDeckProcesses(bool Init)
        {
            try
            {
                List<Process> processes = Process.GetProcessesByName("ElgatoStreamDeckController").ToList();
                if (Init || processes.Count() > 1) // If this is the first run or there are more than one instances running
                {
                    foreach (Process process in processes)
                    {
                        process.Kill(); // Kill all instances
                    }
                    Process.Start(@"ElgatoStreamDeckController.exe"); // Then restart.
                }
                else if (processes.Count() < 1)
                {
                    Process.Start(@"ElgatoStreamDeckController.exe"); // If there are no instances running, start one.
                }
            }
            catch
            {

            }
        }

        // Import functions from user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWindow, StringBuilder StrText, int MaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWindow);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc EnumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWindow, ref Rect Rectangle);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Code for detecting the current position of different windows
        public delegate bool EnumWindowsProc(IntPtr hWindow, IntPtr lParam);

        /// <summary> Get the text for the window pointed to by hWnd </summary>
        public static string GetWindowText(IntPtr hWindow)
        {
            int Size = GetWindowTextLength(hWindow);

            if (Size > 0)
            {
                var Builder = new StringBuilder(Size + 1);
                GetWindowText(hWindow, Builder, Builder.Capacity);
                return Builder.ToString();
            }

            return String.Empty;
        }
        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="Filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        public static IEnumerable<IntPtr> FindWindows(EnumWindowsProc Filter)
        {
            List<IntPtr> Windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr Window, IntPtr Param)
            {
                if (Filter(Window, Param))
                {
                    // Only add the windows that pass the filter.
                    Windows.Add(Window);
                }

                // But return true here so that we iterate all windows.
                return true;

            }, IntPtr.Zero);

            return Windows;
        }
        /// <summary> Find all windows that contain the given title text </summary>
        /// <param name="TitleText"> The text that the window title must contain. </param>
        public static IEnumerable<IntPtr> FindWindowsWithText(string TitleText)
        {
            return FindWindows(delegate (IntPtr Window, IntPtr Param)
            {
                return GetWindowText(Window).Contains(TitleText);
            });
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
