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
        List<object> ButtonList;
        List<object> ButtonList2;
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
                    BButton.Background = Brushes.Black;
                }
        }
    }
}
