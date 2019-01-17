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
            //Motor0.Value = 0;
            //Motor1.Value = 0;
        }

        public void SetLeftMotorValue(double MotorValue)
        {
            Motor0.Value = MotorValue;
        }

        public void SetRightMotorValue(double MotorValue)
        {
            Motor1.Value = MotorValue;
        }

    }
}
