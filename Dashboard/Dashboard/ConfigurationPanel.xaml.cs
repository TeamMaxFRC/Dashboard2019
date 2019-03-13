using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for ConfigurationPanel.xaml
    /// </summary>
    public partial class ConfigurationPanel : UserControl
    {
        public IntPtr DriverStation;

        public ConfigurationPanel()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void StreamDeckButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Loads the FRC driver station.
        public void LoadDriverStation()
        {

            try
            {
                if (ResizeDriverStation() == IntPtr.Zero)
                {
                    Process.Start(@"C:\Program Files (x86)\FRC Driver Station\DriverStation.exe");
                }
            }
            catch
            {

            }

            // Find the driver station.
            DriverStation = ResizeDriverStation();
        }

        // Find the location of the driver station.
        public IntPtr ResizeDriverStation()
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
                            if (DriverStation != IntPtr.Zero)
                            {
                                Application.Current.MainWindow.Height = SystemParameters.WorkArea.Height / System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * Rectangle.Top;
                                SetForegroundWindow(DriverStation);
                            }
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
    }
}
