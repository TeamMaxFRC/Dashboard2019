using System.Windows.Controls;
using System.Windows.Input;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Camera.xaml
    /// </summary>
    public partial class Camera : UserControl
    {
        public Camera()
        {
            InitializeComponent();
            wbSample.Navigate("http://www.wpf-tutorial.com");
        }
    }
}
