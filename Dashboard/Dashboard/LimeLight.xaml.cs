using System.Windows.Controls;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for CurrentMeter.xaml
    /// </summary>
    public partial class LimeLight : UserControl
    {
        public LimeLight()
        {
            InitializeComponent();
        }

        public void RefreshPage()
        {
            bool test = LimeLightWebView.IsLoaded;
            if (test)
            {
                Debug.Content = "Loaded!";
            }
            else
            {
                Debug.Content = "Nope!";
            }
        }
    }
}