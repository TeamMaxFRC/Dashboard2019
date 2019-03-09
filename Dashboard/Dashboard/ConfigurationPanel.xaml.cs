using System.Windows;
using System.Windows.Controls;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for ConfigurationPanel.xaml
    /// </summary>
    public partial class ConfigurationPanel : UserControl
    {
        public ConfigurationPanel()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // private void btnClose_Click(object sender, EventArgs e)
        //  {
        //    Dashboard.close();
        //  }
    }
}
