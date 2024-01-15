using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace C4_WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int NUM_ROWS = 6;
        public const int NUM_COLS = 7;

        public void OnTriangleClicked(object sender)
        {
            int colNumber = ((TriangleButton)sender).myCol;
            grid[0, colNumber].fillColor = Colors.Blue;
        }

        public MainWindow()
        {
            InitializeComponent();
            CustomEvents.TriangleClicked += OnTriangleClicked;
        }
    }
}