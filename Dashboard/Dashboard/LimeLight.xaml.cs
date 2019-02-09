using System;
using System.Windows;
using System.Windows.Controls;
using MjpegProcessor;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Limelight.xaml
    /// </summary>
    /// 

    public partial class Limelight : UserControl
    {
        readonly MjpegDecoder _mjpeg;

        public Limelight()
        {
            InitializeComponent();
            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;
            _mjpeg.Error += _mjpeg_Error;
        }

        public void InitStream()
        {
            _mjpeg.ParseStream(new Uri("http://limelight.local:5800"));
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            MJpegView.Source = e.BitmapImage;
        }

        void _mjpeg_Error(object sender, ErrorEventArgs e)
        {
            _mjpeg.StopStream();
            _mjpeg.ParseStream(new Uri("http://limelight.local:5800"));
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
