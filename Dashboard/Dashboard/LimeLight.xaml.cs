using System;
using System.Windows;
using System.Windows.Controls;
using MjpegProcessor;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Limelight.xaml
    /// </summary>
    /// 

    public partial class Limelight : UserControl
    {
        //private BitmapImage connecting = new BitmapImage(new Uri("images/connect.jpg", UriKind.Relative));
        readonly MjpegDecoder _mjpeg;
        Uri StreamAddress = new Uri("http://10.10.71.205:5800");

        Boolean Errored = false;

        //public bool CurrentlyStreaming = false;
        public Limelight()
        {
            InitializeComponent();
            //MJpegView.Source = connecting;
            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;
            _mjpeg.Error += _mjpeg_Error;
            _mjpeg.ParseStream(StreamAddress);
        }

        public void CheckStream()   
        {
            if(Errored)
            {
                //CurrentlyStreaming = true;
                Errored = false;
                _mjpeg.ParseStream(StreamAddress);
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MJpegView.Source = null;
            _mjpeg.StopStream();
            Errored = false;
            _mjpeg.ParseStream(StreamAddress);
        }
    }
}
