using System;
using System.Windows;
using System.Windows.Controls;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {

        public Console()
        {
            InitializeComponent();
        }

        //Will connect robot to console widget.
      
            public void SetError1(String Message, bool Add)
            if (Add)
           {
               ConsoleBox.Text += Message + "\n";
         
           }
        {
 

        // Prints a line to the console.
        public void PrintLine(String Line)
        {
            ConsoleBox.Text += "\n" + Line;
            ConsoleBox.ScrollToEnd();
        }
    }
}
