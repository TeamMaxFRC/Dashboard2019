using SharpOSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            // Receive loop from Rug OSC.
            while (true)
            {
                try
                {

                    // Get the next message. This receive will not block.
                    OscMessage ReceivedMessage = (OscMessage)Receiver.Receive();

                    if (ReceivedMessage == null) continue;

                    // Show the recieved gyro values
                    if (ReceivedMessage.Address.Equals("/Robot/NavX/Gyro"))
                    {
                        // Print the gyro value every 100 loops.
                        if (PrintTimer % 100 == 0)
                        {
                            Application.Current.Dispatcher.InvokeAsync(new Action(() => ConsoleBox.PrintLine(((float)ReceivedMessage.Arguments[0]).ToString("0.###"))));
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
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/LeftMaster/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetLeftMasterMotorValue((double)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/LeftSlavePrimary/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetLeftSlavePrimaryMotorValue((double)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/LeftSlaveSecondary/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetLeftSlaveSecondaryMotorValue((double)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/RightMaster/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetRightMasterMotorValue((double)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/RightSlavePrimary/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetRightSlavePrimaryMotorValue((double)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/RightSlaveSecondary/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetRightSlaveSecondaryMotorValue((double)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Equals("/Robot/Console/Text"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => ConsoleBox.PrintLine((String)ReceivedMessage.Arguments[0])));
                    }
                    if (ReceivedMessage.Address.Contains("/Robot/Motors/"))
                    {
                        string[] SplitAddress = ReceivedMessage.Address.Split('/');
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => LoggerWidget.SetDataInDataGrid(SplitAddress[3], SplitAddress[4], (double)ReceivedMessage.Arguments[0])));
                    }

                    //if (ReceivedMessage.Address.Contains("/Robot/Error/"))
                    //{
                    //    bool ErrorState = (int)ReceivedMessage.Arguments[0] == 1;
                    //    Application.Current.Dispatcher.InvokeAsync(new Action(() => ErrorWidget.SetError1(ReceivedMessage.Address, ErrorState)));
                    //}

                    // Received Controller Values
                    if (ReceivedMessage.Address.Equals("/Robot/Controller"))
                    {
                        //Application.Current.Dispatcher.InvokeAsync(new Action(() => ControllerDiagnostics.UpdateButtonData((String)ReceivedMessage.Arguments[0])));
                    }

                }
                catch (Exception Ex)
                {
                    // Catch any exceptions.
                    MessageBox.Show(Ex.Message, "OSC Receive Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                }

            }
        }

        private void MainDashboard_Closing(object sender, CancelEventArgs e)
        {
            Receiver.Close();
        }

        private void CurrentWidget_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ConsoleBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
    

}
