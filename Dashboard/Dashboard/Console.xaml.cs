﻿using System;
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

        // Prints a line to the console.
        public void PrintLine(String Line)
        {
            ConsoleBox.Text += "\n" + Line;
            ConsoleBox.ScrollToEnd();
        }

        private void ConsoleBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
         
        private void ConsoleClear_Click(object sender, RoutedEventArgs e)
        {
            ConsoleBox.Text = String.Empty;

        }
    }
}
