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
            Motor0.Value = 0;
            Motor1.Value = 0;
        }

        public void SetLeftMasterMotorValue(double MotorValue0)
        {
            Motor0.Value = MotorValue0;
        }
        public void SetLeftSlavePrimaryMotorValue(double MotorValue1)
        {
            Motor1.Value = MotorValue1;
        }
        public void SetLeftSlaveSecondaryMotorValue(double MotorValue2)
        {
            Motor2.Value = MotorValue2;
        }
        public void SetRightMasterMotorValue(double MotorValue13)
        {
            Motor13.Value = MotorValue13;
        }
        public void SetRightSlavePrimaryMotorValue(double MotorValue14)
        {
            Motor14.Value = MotorValue14;
        }
        public void SetRightSlaveSecondaryMotorValue(double MotorValue15)
        {
            Motor15.Value = MotorValue15;
        }

        public void SetCurrentMeter(double SubsystemCurrent, string Subsystem)
        {

            switch (Subsystem)
            {

                case ("/DriveCurrent"):
                    Motor0.Value = SubsystemCurrent;
                    break;

                case ("/LiftCurrent"):
                    Motor1.Value = SubsystemCurrent;
                    break;

                case ("/FourBarCurrent"):
                    Motor2.Value = SubsystemCurrent;
                    break;

                case ("/GathererCurrent"):
                    Motor13.Value = SubsystemCurrent;
                    break;

                case ("/CompressorCurrent"):
                    Motor14.Value = SubsystemCurrent;
                    break;

                default:
                    break;

            }

            TotalCurrent.Value = Motor0.Value + Motor1.Value + Motor2.Value + Motor13.Value + Motor14.Value;

        }

    }

}
