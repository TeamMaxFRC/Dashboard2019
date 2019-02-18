﻿using SharpOSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Drawing;

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
        public IntPtr DriverStation;
        public Rect DriverStationRect;

        // Background worker which will receive the OSC data.
        private BackgroundWorker UpdateWorker = new BackgroundWorker();

        // Create the OSC receiver.
        private static UDPListener Receiver;

        public MainWindow()
        {
            InitializeComponent();
            LoadDriverStation();

            // Connect the receiver to the proper port.
            Receiver = new UDPListener(5803);

            // Link the update method to the background worker.
            UpdateWorker.DoWork += Update;

            // Have the update worker run in its own thread.
            UpdateWorker.RunWorkerAsync();

            // Resze the dashboard based on the screen resolution.


        }

        public void Update(object sender, DoWorkEventArgs e)
        {
            //LimelightWidget.InitStream();

            // Receive loop from Sharp OSC.
            while (true)
            {

                try
                {

                    // Receive the packet, but skip if it's null.
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

                        // Show any recieved Limelight values.
                        if (ReceivedMessage.Address.Equals("/Robot/Limelight/X"))
                        {
                            //Application.Current.Dispatcher.InvokeAsync(new Action(() => LimelightWidget.UpdateX((double)ReceivedMessage.Arguments[0])));
                        }
                        if (ReceivedMessage.Address.Equals("/Robot/Limelight/Y"))
                        {
                            //Application.Current.Dispatcher.InvokeAsync(new Action(() => LimelightWidget.UpdateY((double)ReceivedMessage.Arguments[0])));
                        }
                        if (ReceivedMessage.Address.Equals("/Robot/Limelight/A"))
                        {
                            //Application.Current.Dispatcher.InvokeAsync(new Action(() => LimelightWidget.UpdateA((double)ReceivedMessage.Arguments[0])));
                        }

                        // Show any received motor values.
                        if (ReceivedMessage.Address.Equals("/Robot/Console/Text"))
                        {
                            // Application.Current.Dispatcher.InvokeAsync(new Action(() => ConsoleBox.PrintLine((String)ReceivedMessage.Arguments[0])));
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

                            // Create the relevant data for the current tracker.
                            string BundleIdentifier = "";

                            // Iterate through all the messages and generate the header and data rows.
                            foreach (OscMessage Message in Bundle.Messages)
                            {

                                if (Message.Address.Equals("/BundleIdentifier"))
                                {
                                    BundleIdentifier = (string)Message.Arguments[0];
                                }
                                else
                                {
                                    Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetCurrentMeter((double)Message.Arguments[0], Message.Address)));
                                }

                            }

                            // TODO: Error bundle handling.

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

        // Loads the FRC driver station.
        private void LoadDriverStation()
        {

            try
            {
                Process.Start(@"C:\Program Files (x86)\FRC Driver Station\DriverStation.exe");
            }
            catch
            {

            }

            // Find the driver station.
            DriverStation = FindDriverStation();
        }

        // Find the location of the driver station.
        public IntPtr FindDriverStation()
        {
            try
            {
                List<IntPtr> PossibleWindows = FindWindowsWithText("FRC Driver Station").ToList();

                if (PossibleWindows.Count != 0)
                {
                    
                    Rect Rectangle = new Rect();

                    foreach (IntPtr Window in PossibleWindows)
                    {
                        GetWindowRect(Window, ref Rectangle);

                        if (Rectangle.Left == 0 && Rectangle.Right >= SystemParameters.PrimaryScreenWidth - 1 && Rectangle.Top > SystemParameters.PrimaryScreenHeight * .5)
                        {
                            DriverStationRect = Rectangle;
                            return Window;
                        }
                        
                    }

                    return IntPtr.Zero;

                }
                else
                {
                    return IntPtr.Zero;
                }

            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        private void MainDashboard_Closing(object sender, CancelEventArgs e)
        {
            Receiver.Close();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DriverStation = FindDriverStation();
            if (DriverStation != IntPtr.Zero)
            {
                MainDashboard.Left = 0;
                MainDashboard.Top = 0;
                MainDashboard.Width = SystemParameters.PrimaryScreenWidth;
                double test = SystemParameters.PrimaryScreenHeight;
                double Test = DriverStationRect.Top - 1;
                MainDashboard.Height = SystemParameters.WorkArea.Height / System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * DriverStationRect.Top;
            }
        }
    }
}
