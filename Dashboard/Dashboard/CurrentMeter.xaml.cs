using System.Windows.Controls;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for CurrentMeter.xaml
    /// </summary>
    public partial class CurrentMeter : UserControl
    {
        public CurrentMeter()
        {
            InitializeComponent();
        }

        public void SetCurrentMeter(double SubsystemCurrent, string Subsystem)
        {

            // For each subsystem, set the current in the bar and the text.
            switch (Subsystem)
            {

                case ("/DriveCurrent"):
                    Drive.Value = SubsystemCurrent;
                    DriveValue.Text = SubsystemCurrent.ToString() + "A";
                    break;

                case ("/LiftCurrent"):
                    Lift.Value = SubsystemCurrent;
                    LiftValue.Text = SubsystemCurrent.ToString() + "A";
                    break;

                case ("/FourBarCurrent"):
                    FourBar.Value = SubsystemCurrent;
                    FourBarValue.Text = SubsystemCurrent.ToString() + "A";
                    break;

                case ("/GathererCurrent"):
                    Gatherer.Value = SubsystemCurrent;
                    GathererValue.Text = SubsystemCurrent.ToString() + "A";
                    break;

                case ("/CompressorCurrent"):
                    Compressor.Value = SubsystemCurrent;
                    CompressorValue.Text = SubsystemCurrent.ToString() + "A";
                    break;

                default:
                    break;

            }

            // Set the total current to the sum of all the values.
            Total.Value = Drive.Value + Lift.Value + FourBar.Value + Gatherer.Value + Compressor.Value;
            TotalValue.Text = Total.Value.ToString() + "A";

        }

    }

}
