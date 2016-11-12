using System;
using System.Numerics;
using Windows.Foundation;

namespace AsteroidsUWP.Core
{
    public static class PolygonHelper
    {
        public static Vector2[] Rotate(this Vector2[] polygon, Vector2 centroid, double angle)
        {
            for (int i = 0; i < polygon.Length; i++)
            {
                polygon[i] = RotatePoint(polygon[i], centroid, angle);
            }

            return polygon;
        }

        private static Vector2 RotatePoint(Vector2 point, Vector2 centroid, double angle)
        {
            var x = centroid.X + (float)((point.X - centroid.X) * Math.Cos(angle) - (point.Y - centroid.Y) * Math.Sin(angle));
            var y = centroid.Y + (float)((point.X - centroid.X) * Math.Sin(angle) + (point.Y - centroid.Y) * Math.Cos(angle));

            return new Vector2(x, y);
        }

        public static Vector2[] ClonePolygon(this Vector2[] points)
        {
            Vector2[] clone = new Vector2[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                clone[i] = points[i];
            }

            return clone;
        }

        //http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        public static bool IsPointInPolygon(this Vector2[] polygon, Vector2 testPoint)
        {
            int i, j;
            bool isInside = false;
            int numberOfPoints = polygon.Length;

            for (i = 0, j = numberOfPoints - 1; i < numberOfPoints; j = i++)
            {
                if (((polygon[i].Y > testPoint.Y) != (polygon[j].Y > testPoint.Y)) &&
                    (testPoint.X < (polygon[j].X - polygon[i].X) * (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                    isInside = !isInside;
            }

            return isInside;
        }

        public static bool CollidesWith(this Vector2[] polygon, Vector2[] otherPolygon)
        {
            for (int i = 0; i < polygon.Length; i++)
            { 
                if(otherPolygon.IsPointInPolygon(polygon[i]))
                    return true;
            }

            return false;
        }
    }
}