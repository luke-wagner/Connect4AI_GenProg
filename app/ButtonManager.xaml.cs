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
    /// Interaction logic for ButtonManager.xaml
    /// </summary>
    /// #check -- unecessary User Control?
    public partial class ButtonManager : UserControl
    {
        public ButtonManager()
        {
            InitializeComponent();
            CustomEvents.TriangleClicked += onTriangleClicked;
        }

        private void onTriangleClicked(object sender)
        {

        }
    }
}
