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
                this.Ellipse.Fill = new SolidColorBrush(value);
            }
        }

        public GridItem(int x, int y)
        {
            InitializeComponent();
            SetValue(Grid.RowProperty, x);
            SetValue(Grid.ColumnProperty, y);
        }
    }
}
