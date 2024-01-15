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

namespace C4_WPFApp
{
    /// <summary>
    /// Interaction logic for Grid.xaml
    /// </summary>
    public partial class C4Grid : UserControl
    {
        private GridItem[,] _grid = new GridItem[6, 7];

        public ref GridItem this[int i, int j]
        {
            get
            {
                return ref _grid[i, j];
            }
        }

        public C4Grid()
        {
            InitializeComponent();
            for (int i = 0; i < MainWindow.NUM_ROWS; i++)
            {
                for(int j = 0; j < MainWindow.NUM_COLS; j++)
                {
                    _grid[i, j] = new GridItem(i, j);
                    root.Children.Add(_grid[i, j]);
                }
            }
        }
    }
}
