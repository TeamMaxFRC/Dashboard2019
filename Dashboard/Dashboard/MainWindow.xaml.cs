using Rug.Osc;
using System;
using System.ComponentModel;
using System.Windows;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Background worker which will receive the OSC data.
        private BackgroundWorker UpdateWorker = new BackgroundWorker();

        // OSC receiver instance.
        private static OscReceiver Receiver = new OscReceiver(5801);

        public MainWindow()
        {
            InitializeComponent();

            // Connect the OSC receiver socket to the RIO.
            Receiver.Connect();

            // Link the update method to the background worker.
            UpdateWorker.DoWork += Update;

            // Have the update worker run in its own thread.
            UpdateWorker.RunWorkerAsync();

        }

        public void Update(object sender, DoWorkEventArgs e)
        {

            // Receive loop from Rug OSC.
            try
            {

                while (Receiver.State != OscSocketState.Closed)
                {

                    if (Receiver.State == OscSocketState.Connected)
                    {

                        // Get the next message. This receive will block until data appears.
                        OscPacket Packet = Receiver.Receive();

                        // Split the string into a path and a value.
                        string[] OscArray = Packet.ToString().Split(',');

                        string Path = OscArray[0];
                        string Value = OscArray[1];

                        // Show any received motor values.
                        if (Path.Equals("/Robot/Motors/Left/Value"))
                        {
                            CurrentWidget.SetLeftMotorValue(double.Parse(Value.Remove(Value.Length-1), System.Globalization.NumberStyles.AllowLeadingWhite));
                        }

                        if (Path.Equals("/Robot/Motors/Right/Value"))
                        {
                            // CurrentWidget.SetRightMotorValue(double.Parse(Value.Remove(Value.Length - 1), System.Globalization.NumberStyles.AllowLeadingWhite));
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                // Catch any exceptions.
            }


        }

    }

}
