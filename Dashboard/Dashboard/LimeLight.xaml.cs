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
    /// Interaction logic for Limelight.xaml
    /// </summary>
    public partial class Limelight : UserControl
    {
        public Limelight()
        {
            InitializeComponent();
        }

        public void UpdateX(double X)
        {
            xDisplay.Text = "X: " + X.ToString();
        }

        public void UpdateY(double Y)
        {
            yDisplay.Text = "Y: " + Y.ToString();
        }

        public void UpdateA(double A)
        {
            aDisplay.Text = "A: " + A.ToString();
        }
    }
}
