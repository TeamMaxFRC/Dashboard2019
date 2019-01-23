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
    }
}
