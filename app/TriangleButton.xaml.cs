using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace C4_WPFApp
{
    /// <summary>
    /// Interaction logic for TriangleButton.xaml
    /// </summary>
    public partial class TriangleButton : UserControl
    {
        private int _myCol;
        public int myCol
        {
            get => _myCol;
            set
            {
                _myCol = value;
            }
        }

        public TriangleButton()
        {
            InitializeComponent();
        }

        private async void ClickAnimation()
        {
            /*
            PointCollection myPoints = new PointCollection() { 
                new Point(0, 100),
                new Point (100, 100),
                new Point(50, 0)
            };
            root.Points = myPoints;
            */
            // Scaling up portion of animation
            root.Fill = new SolidColorBrush(Colors.Gold);
            for (int i = 1; i < 10; i++)
            {
                await Task.Delay(10);
                HelperFuncts.ScalePolygon(ref root, 1.03);
            }
            for (int i = 1; i < 10; i++)
            {
                await Task.Delay(10);
                HelperFuncts.ScalePolygon(ref root, 0.97);
            }
            root.Fill = new SolidColorBrush(Colors.CornflowerBlue);
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            CustomEvents.InvokeTriangleClicked(this);
            ClickAnimation();
        }
    }
}
