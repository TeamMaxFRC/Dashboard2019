using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ErrorReporter.xaml
    /// </summary>
    public partial class ErrorReporter : UserControl
    {

        ObservableCollection<string> SelectedItem = new ObservableCollection<string>();

        public ErrorReporter()
        {
            InitializeComponent();
            ErrorBox.ItemsSource = SelectedItem;

        }

        private void ErrorButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem.Add(ErrorTransporter.Text);
        }

        private void ErrorRemoval_Click(object sender, RoutedEventArgs e)
        {
            //ErrorList.Remove(ErrorTransporter.Text);
    
            SelectedItem.Remove((string)ErrorBox.SelectedItem);
        }

    }

}
