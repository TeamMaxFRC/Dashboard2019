using SharpOSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

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

        // Background worker which will receive the OSC data.
        private BackgroundWorker UpdateWorker = new BackgroundWorker();

        // Create the OSC receiver.
        private static UDPListener Receiver;

        public MainWindow()
        {
            InitializeComponent();

            // Connect the receiver to the proper port.
            Receiver = new UDPListener(5803);

            // Link the update method to the background worker.
            UpdateWorker.DoWork += Update;

            // Have the update worker run in its own thread.
            UpdateWorker.RunWorkerAsync();

        }

        public void Update(object sender, DoWorkEventArgs e)
        {
            // Time before another console print occurs.
            int PrintTimer = 0;
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

                        // Show the recieved gyro values
                        if (ReceivedMessage.Address.Equals("/Robot/NavX/Gyro"))
                        {
                            // Print the gyro value every 100 loops.
                            if (PrintTimer % 100 == 0)
                            {
                                // Application.Current.Dispatcher.InvokeAsync(new Action(() => ConsoleBox.PrintLine(((float)ReceivedMessage.Arguments[0]).ToString("0.###"))));
                            }

                            // Increment the print timer.
                            PrintTimer++;
                        }
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

                        if (ReceivedMessage.Address.Contains("/Robot/Error/"))
                        {
                            bool ErrorState = (int)ReceivedMessage.Arguments[0] == 1;
                            // Application.Current.Dispatcher.InvokeAsync(new Action(() => ErrorWidget.SetError1(ReceivedMessage.Address, ErrorState)));
                        }

                        // Received Controller Values
                        if (ReceivedMessage.Address.Equals("/Robot/Controller"))
                        {
                            //Application.Current.Dispatcher.InvokeAsync(new Action(() => ControllerDiagnostics.UpdateButtonData((String)ReceivedMessage.Arguments[0])));
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

        private void MainDashboard_Closing(object sender, CancelEventArgs e)
        {
            Receiver.Close();
        }

    }

}
