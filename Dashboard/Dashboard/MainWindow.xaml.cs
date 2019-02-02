﻿using SharpOSC;
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
            Receiver = new UDPListener(5801);

            // Link the update method to the background worker.
            UpdateWorker.DoWork += Update;

            // Have the update worker run in its own thread.
            UpdateWorker.RunWorkerAsync();

        }

        public void Update(object sender, DoWorkEventArgs e)
        {



            // Receive loop from Rug OSC.
            while (true)
            {
                try
                {
                    // Get the next message. This receive will not block.
                    OscMessage ReceivedMessage = (OscMessage)Receiver.Receive();

                    if (ReceivedMessage == null) continue;

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
                    //if (RecievedMessaage.Address.Equals("Error"))
                    //Application.ErrorReporter.Dispatcher.InvokeAsync(new Action(() => ErrorReporter.SendError((double)Arguments[0])));

                    //if (ReceivedMessage.Address.Contains("/Robot/Error/"))
                    //{
                    //    bool ErrorState = (int)ReceivedMessage.Arguments[0] == 1;
                    //    Application.Current.Dispatcher.InvokeAsync(new Action(() => ErrorWidget.SetError1(ReceivedMessage.Address, ErrorState)));
                    //}

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
        
    }
    

}
