using System.Windows;
using System.Windows.Controls;
using MouseJiggler.Properties;

namespace MouseJiggler;

public class JigglePattern
{
    private readonly Point[] _points;

    public static JigglePattern Create(JiggleMode mode, int size)
    {
        switch (mode)
        {
            case JiggleMode.Zen:
                return new JigglePattern(new Point(0, 0));
            case JiggleMode.ZigZag:
                return new JigglePattern(new Point(size, size), new Point(-size, -size));
            case JiggleMode.Circle:
                return new JigglePattern(ComputeCirclePoints(size));
            case JiggleMode.Horizontal:
                return new JigglePattern(ComputeLinearPoints(size, Orientation.Horizontal));
            case JiggleMode.Vertical:
                return new JigglePattern(ComputeLinearPoints(size, Orientation.Vertical));
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    private static Point[] ComputeLinearPoints(int size, Orientation orientation)
    {
        var points = new List<Point>(8);
        int delta = size / 5;

        // somewhat to the right / down
        for (int i = 0; i < 3; i++)
        {
            points.Add(orientation == Orientation.Horizontal
                           ? new Point(delta, 0)
                           : new Point(0, delta));
        }

        // double the way to the left / up
        for (int i = 0; i < 6; i++)
        {
            points.Add(orientation == Orientation.Horizontal
                           ? new Point(-delta, 0)
                           : new Point(0, -delta));
        }

        // return back to the right / down
        for (int i = 0; i < 3; i++)
        {
            points.Add(orientation == Orientation.Horizontal
                           ? new Point(delta, 0)
                           : new Point(0, delta));
        }

        return points.ToArray();
    }

    private static Point[] ComputeCirclePoints(int size)
    {
        const int pointCount = 8;
        var circlePoints = new List<Point>(pointCount);

        double radius = size / 2.0f;

        var points = new List<Point>(pointCount);
        for (int i = 0; i < pointCount; i++)
        {
            double angle = 2.0f * Math.PI * i / pointCount;
            int dx = (int)Math.Round(radius * Math.Cos(angle));
            int dy = (int)Math.Round(radius * Math.Sin(angle));
            points.Add(new Point(dx, dy));
        }

        // Save the first circle point
        circlePoints.Add(points[0]);

        // Save the differences (deltas) between consecutive points,
        // including the jump back from the last point to the first point.
        for (int i = 0; i < pointCount; i++)
        {
            Point current = points[i];
            Point next = points[(i + 1) % pointCount];
            circlePoints.Add(new Point(next.X - current.X, next.Y - current.Y));
        }

        return circlePoints.ToArray();
    }

    private JigglePattern(params Point[] points)
    {
        _points = points;
    }

    public void Perform()
    {
        Helpers.SavePos();

        foreach (Point p in _points)
        {
            Helpers.Jiggle((int)p.X, (int)p.Y);
            Thread.Sleep(5);
        }

        Helpers.RestorePos();
    }
}