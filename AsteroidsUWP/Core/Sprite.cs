using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using AsteroidsUWP.GameObjects;
using Microsoft.Graphics.Canvas.Geometry;

namespace AsteroidsUWP.Core
{
    public class Sprite
    {
        public Sprite()
        {
            Location = new Vector2(0,0);
            Polygon = new[]{new Vector2(0,0), new Vector2(0, 0), new Vector2(0, 0),  };
        }

        public Vector2 Location { get; set; }
        public Vector2[] Polygon { get; set; }
        public CanvasGeometry PolygonGeometry { get; set; }
        public float Speed { get; set; }
        public double TravelDirectionInDegrees { get; set; }
        public double DirectionOfSprite { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }

        public bool IsPointWithin(Vector2 point)
        {
            var offsetPoint = point + Location;
            return Polygon.IsPointInPolygon(offsetPoint);
        }

        internal bool CollidesWith(Sprite sprite)
        {
            var offsetPolygon = sprite.Polygon.ClonePolygon();
            for (int i = 0; i < offsetPolygon.Length; i++)
                offsetPolygon[i] += sprite.Location;
            return offsetPolygon.CollidesWith(this.Polygon);
        }

        public List<Line> ToLineList()
        {
            List<Line> lines = new List<Line>();

            for(int i = 0; i < Polygon.Length-1; i++)
            {
                lines.Add(
                    new Line
                        {
                            StartPoint = new Vector2(Polygon[i].X, Polygon[i].Y),
                            EndPoint = new Vector2(Polygon[i + 1].X, Polygon[i + 1].Y)
                        });
                
            }

            return lines;
        }
    }
}