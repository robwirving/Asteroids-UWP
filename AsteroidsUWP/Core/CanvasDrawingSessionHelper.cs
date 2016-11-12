using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.Core
{
    public static class CanvasDrawingSessionHelper
    {
        public static void DrawPolygon(this CanvasDrawingSession graphics, Vector2[] points, Color color)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                graphics.DrawLine(points[i+0], points[i+1], color);
            }
            graphics.DrawLine(points[points.Length-1], points[0], color);
        }
    }
}
