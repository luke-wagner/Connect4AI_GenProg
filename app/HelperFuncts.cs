using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace C4_WPFApp;

public abstract class HelperFuncts
{
    public static void ScalePolygon(ref Polygon shape, double scaleFactor)
    {
        int numPoints = 0;
        double cumulativeX = 0;
        double cumultaiveY = 0;
        foreach (var point in shape.Points)
        {
            cumulativeX += point.X;
            cumultaiveY += point.Y;
            numPoints++;
        }
        double averageX = cumulativeX / numPoints;
        double averageY = cumultaiveY / numPoints;
        Point centerPoint = new Point(averageX, averageY);

        PointCollection newPoints = new PointCollection();
        foreach (var point in shape.Points)
        {
            Vector distFromCenter = Point.Subtract(point, centerPoint);
            Point distFromCenterScaled = new Point(distFromCenter.X * scaleFactor, distFromCenter.Y * scaleFactor);
            newPoints.Add(Point.Add(centerPoint, (Vector)distFromCenterScaled));
        }

        shape.Points = newPoints;
    }
}