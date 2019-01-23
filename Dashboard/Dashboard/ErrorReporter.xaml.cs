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

        ObservableCollection<string> ErrorList = new ObservableCollection<string>();

        public ErrorReporter()
        {
            InitializeComponent();
            ErrorBox.ItemsSource = ErrorList;

        }

        private void ErrorButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorList.Add(ErrorTransporter.Text);
        }

        private void ErrorRemoval_Click(object sender, RoutedEventArgs e)
        {
            //ErrorList.Remove(ErrorTransporter.Text);

            ErrorList.Remove((string)ErrorBox.SelectedItem);
        }

        public void SetError1(String Message, bool Add)
        {

            if (Add)
            {
                if (!ErrorList.Contains(Message.Replace("/Robot/Error/", "")))
                {
                    ErrorList.Add(Message.Replace("/Robot/Error/", ""));
                }
            }
            else
            {
                ErrorList.Remove(Message.Replace("/Robot/Error/", ""));
            }

        }

    }

}
