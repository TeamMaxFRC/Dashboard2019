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
    /// Interaction logic for UserControl1.xaml
    /// </summary>


    public partial class Console : UserControl
    {
        public Console()
        {
            InitializeComponent();
        }

        //
        public void PrintLine(String Line)
        {

            ConsoleBox.Text += "\n" + Line;

        }

        private void test()
        {
            PrintLine("Graphing Paper");

            double gyro = 0.75;
            PrintLine(gyro.ToString());
        }
    }
}
