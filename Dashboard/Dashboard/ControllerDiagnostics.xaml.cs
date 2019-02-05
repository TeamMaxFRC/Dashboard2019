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
    /// Interaction logic for ControllerDiagnostics.xaml
    /// </summary>
    public partial class ControllerDiagnostics : UserControl
    {
        public ControllerDiagnostics()
        {
            InitializeComponent();
        }

        public void UpdateButtonData(ControllerData DataForButtons0, ControllerData DataForButtons1, ControllerData DataForButtons2, ControllerData DataForButtons3, ControllerData DataForButtons4, ControllerData DataForButtons5, ControllerData DataForButtons6, ControllerData DataForButtons7, ControllerData DataForButtons8, ControllerData DataForButtons9, ControllerData DataForButtons10, ControllerData DataForButtons11, ControllerData DataForButtons12, ControllerData DataForButtons13, ControllerData DataForButtons14, ControllerData DataForButtons15, ControllerData DataForButtons16, ControllerData DataForButtons17, ControllerData DataForButtons18, ControllerData DataForButtons19, ControllerData DataForButtons20, ControllerData DataForButtons21, ControllerData DataForButtons22, ControllerData DataForButtons23, ControllerData DataForButtons24, ControllerData DataForButtons25)
        {
                if (DataForButtons0.AButton)
                {
                    AButton.Background = Brushes.LawnGreen;
                }
                else
                {
                AButton.Background = Brushes.GhostWhite;
                }
                if (DataForButtons1.BButton)
                {
                    BButton.Background = Brushes.IndianRed;
                }
                else 
                {
                   BButton.Background = Brushes.GhostWhite;
                }
            if (DataForButtons2.XButton)
            {
                XButton.Background = Brushes.CornflowerBlue;
            }
            else
            {
                XButton.Background = Brushes.GhostWhite;
            }
            if (DataForButtons3.YButton)
            {
                YButton.Background = Brushes.Yellow;
            }
            else
            {
                YButton.Background = Brushes.GhostWhite;
            }
            if (DataForButtons4.DPadUp)
            {
                DPadUp.Background = Brushes.Black;
            }
            else
            {
                DPadUp.Background = Brushes.GhostWhite;
            }
            if (DataForButtons5.DPadDown)
            {
                DPadDown.Background = Brushes.Black;
            }
            else
            {
                DPadDown.Background = Brushes.GhostWhite;
            }
            if (DataForButtons6.DPadLeft)
            {
                DPadLeft.Background = Brushes.Black;
            }
            else
            {
                DPadLeft.Background = Brushes.GhostWhite;
            }
            if (DataForButtons7.DPadRight)
            {
                DPadRight.Background = Brushes.Black;
            }
            else
            {
                DPadRight.Background = Brushes.GhostWhite;
            }
            if (DataForButtons8.MenuButton)
            {
                MenuButton.Background = Brushes.Black;
            }
            else
            {
                MenuButton.Background = Brushes.GhostWhite;
            }
            if (DataForButtons9.ViewButton)
            {
                ViewButton.Background = Brushes.Black;
            }
            else
            {
                ViewButton.Background = Brushes.GhostWhite;
            }
            if (DataForButtons10.XboxButton)
            {
                XboxButton.Background = Brushes.Black;
            }
            else
            {
                XboxButton.Background = Brushes.GhostWhite;
            }
            if (DataForButtons11.LeftBumper)
            {
                LeftBumper.Fill = Brushes.Black;
            }
            else
            {
                LeftBumper.Fill = Brushes.GhostWhite;
            }
            if (DataForButtons12.RightBumper)
            {
                RightBumper.Fill = Brushes.Black;
            }
            else
            {
                RightBumper.Fill = Brushes.GhostWhite;
            }
            if (DataForButtons13.AButton2)
            {
                AButton2.Background = Brushes.LawnGreen;
            }
            else
            {
                AButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons14.BButton2)
            {
                BButton2.Background = Brushes.IndianRed;
            }
            else
            {
                BButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons15.XButton2)
            {
                XButton2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                XButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons16.YButton2)
            {
                YButton2.Background = Brushes.Yellow;
            }
            else
            {
                YButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons17.DPadUp2)
            {
                DPadUp2.Background = Brushes.Black;
            }
            else
            {
                DPadUp2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons18.DPadDown2)
            {
                DPadDown2.Background = Brushes.Black;
            }
            else
            {
                DPadDown2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons19.DPadLeft2)
            {
                DPadLeft2.Background = Brushes.Black;
            }
            else
            {
                DPadLeft2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons20.DPadRight2)
            {
                DPadRight2.Background = Brushes.Black;
            }
            else
            {
                DPadRight2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons21.MenuButton2)
            {
                MenuButton2.Background = Brushes.Black;
            }
            else
            {
                MenuButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons22.ViewButton)
            {
                ViewButton2.Background = Brushes.Black;
            }
            else
            {
                ViewButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons23.XboxButton2)
            {
                XboxButton2.Background = Brushes.Black;
            }
            else
            {
                XboxButton2.Background = Brushes.GhostWhite;
            }
            if (DataForButtons24.LeftBumper2)
            {
                LeftBumper2.Fill = Brushes.Black;
            }
            else
            {
                LeftBumper2.Fill = Brushes.GhostWhite;
            }
            if (DataForButtons25.RightBumper2)
            {
                RightBumper2.Fill = Brushes.Black;
            }
            else
            {
                RightBumper2.Fill = Brushes.GhostWhite;
            }

        }
    }
}
