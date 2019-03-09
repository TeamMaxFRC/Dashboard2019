using System;
using System.Windows;
using System.Windows.Controls;
using MjpegProcessor;
using System.Threading;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Limelight.xaml
    /// </summary>
    /// 

    public partial class Limelight : UserControl
    {
        readonly MjpegDecoder _mjpeg;
        Uri StreamAddress = new Uri("http://10.10.71.205:5800");

        Boolean Errored;

        //public bool CurrentlyStreaming = false;
        public Limelight()
        {
            InitializeComponent();
            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;
            _mjpeg.Error += _mjpeg_Error;
        }

        public void StartStream()
        {
            if(Errored)
            {
                //CurrentlyStreaming = true;
                Errored = false;
                _mjpeg.ParseStream(StreamAddress);
                Thread.Sleep(5000);
            }
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            MJpegView.Source = e.BitmapImage;
        }

        void _mjpeg_Error(object sender, ErrorEventArgs e)
        {
            _mjpeg.StopStream();
            Errored = true;
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
