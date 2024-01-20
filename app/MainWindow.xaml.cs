using System.Reflection;
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
        C4GUI game = new C4GUI(false);

        private bool placeToken(int colNumber, bool isPlayerToken)
        {
            if (isPlayerToken)
            { 
                (int x, int y) = game.LastMoveCoords;
                grid[x, y].fillColor = Colors.Blue;
                grid[x, y].SlideIn();
            } else
            {
                (int x, int y) = game.LastMoveCoords;
                grid[x, y].fillColor = Colors.Red;
                grid[x, y].SlideIn();
            }
            return true;
        }

        private bool playerMove (object sender)
        {
            (int x, int y) priorLastMove = game.LastMove;
            int colNumber = ((TriangleButton)sender).myCol;
            game.OpponentMove(colNumber);
            if (game.LastMove != priorLastMove)
            {
                placeToken(colNumber, true);
                Application.Current.Properties["CanPlay"] = false;
                return true;
            } else
            {
                return false;
            }
        }

        private async Task computerMove()
        {
            await Task.Delay(1000);
            int computerMove = game.Move();
            placeToken(computerMove, false);
            Application.Current.Properties["CanPlay"] = true;
        }

        public void OnTriangleClicked(object sender)
        {
            if ((bool)Application.Current.Properties["CanPlay"] == false)
            {
                return;
            }
            if (playerMove(sender))
            {
                computerMove();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CustomEvents.TriangleClicked += OnTriangleClicked;
        }
    }
}