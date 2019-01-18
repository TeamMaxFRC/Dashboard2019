using SharpOSC;
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

        // Create and connect the OSC receiver instance.
        private static UDPListener Receiver = new UDPListener(5801);

        public MainWindow()
        {
            InitializeComponent();

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
                    if (ReceivedMessage.Address.Equals("/Robot/Motors/Left/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetLeftMotorValue((double)ReceivedMessage.Arguments[0])));
                    }

                    if (ReceivedMessage.Address.Equals("/Robot/Motors/Right/Value"))
                    {
                        Application.Current.Dispatcher.InvokeAsync(new Action(() => CurrentWidget.SetRightMotorValue((double)ReceivedMessage.Arguments[0])));
                    }

                }
                catch (Exception Ex)
                {
                    // Catch any exceptions.
                    MessageBox.Show(Ex.Message, "OSC Receive Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

    }

}
