using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace C4_WPFApp
{
    /// <summary>
    /// Interaction logic for GridItem.xaml
    /// </summary>
    public partial class GridItem : UserControl
    {
        private Color _fillColor;
        public Color fillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
                if (value == Colors.Blue)
                {
                    Ellipse1.Fill = new SolidColorBrush(Colors.Blue);
                    Ellipse2.Fill = new SolidColorBrush(Colors.DodgerBlue);
                } else if (value == Colors.Red)
                {
                    Ellipse1.Fill = new SolidColorBrush(Colors.DarkRed);
                    Ellipse2.Fill = new SolidColorBrush(Colors.Red);
                }
            }
        }

        public GridItem(int x, int y)
        {
            InitializeComponent();
            SetValue(Grid.RowProperty, x);
            SetValue(Grid.ColumnProperty, y);
        }

        public async void SlideIn()
        {
            // initValue = numIncrements * increment

            Canvas.SetBottom(token, 200);
            for (int i = 0; i < 40; i++)
            {
                await Task.Delay(10);
                Canvas.SetBottom(token, Canvas.GetBottom(token) - 5);
            }
        }
    }
}
