using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
