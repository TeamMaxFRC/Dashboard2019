using System.Windows.Controls;

namespace Dashboard
{

    /// <summary>
    /// Interaction logic for SensorInput.xaml
    /// </summary>
    public partial class SensorInput : UserControl
    {

        public SensorInput()
        {
            InitializeComponent();
        }

        public void SetSensorValue(double SensorValue, string SensorAddress)
        {

            // For each subsystem, set the current in the bar and the text.
            switch (SensorAddress)
            {

                case ("/LiftEncoderVelocity"):
                    LiftEncoderVelocityText.Text = SensorValue.ToString("0.##");
                    break;

                case ("/LiftEncoderPosition"):
                    LiftEncoderPositionText.Text = SensorValue.ToString("0.##");
                    break;

                case ("/LeftEncoderVelocity"):
                    LeftEncoderVelocityText.Text = SensorValue.ToString("0.##");
                    break;

                case ("/RightEncoderVelocity"):
                    RightEncoderVelocityText.Text = SensorValue.ToString("0.##");
                    break;

                case ("/MagneticGatherEncoder"):
                    bool HatchGrabbed = SensorValue == 1 ? true : false;
                    MagneticGatherEncoderText.Text = HatchGrabbed.ToString();
                    break;

                case ("/FourBarEncoderRelativePosition"):
                    FourBarEncoderAbsoluteDegreeText.Text = SensorValue.ToString("0.##");
                    break;

                case ("/FourBarEncoderAbsolutePosition"):
                    FourBarEncoderAbsolutePositionText.Text = SensorValue.ToString("0.##");
                    break;

                default:
                    break;

            }

        }

    }
}
