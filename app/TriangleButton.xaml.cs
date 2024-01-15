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

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            root.Fill = new SolidColorBrush(Colors.Red);
            CustomEvents.InvokeTriangleClicked(this);
        }
    }
}
