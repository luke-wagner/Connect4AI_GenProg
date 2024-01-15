using System.Windows;

namespace C4_WPFApp;

abstract class CustomEvents
{
    public delegate void ButtonClick(object sender);
    public static event ButtonClick TriangleClicked;
    public static void InvokeTriangleClicked(object sender)
    {
        TriangleClicked.Invoke(sender);
    }
}