using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.GameObjects
{
    public class ShipBlowUpScene
    {
        private List<Line> Lines;
        private List<Vector2> _deltas;
        Random _random = new Random((int)DateTime.Now.Ticks);

 
        public void Start(PlayerShip ship)
        {
            Lines = ship.Sprite.ToLineList();

            CreateDeltas();
        }

        private void CreateDeltas()
        {
            _deltas = new List<Vector2>();

            for(int i = 0; i < Lines.Count; i++)
            {
                var point = new Vector2();

                point.X = _random.Next(6) - 3;
                point.Y = _random.Next(6) - 3;
                _deltas.Add(point);
            }
        }

        public void End()
        {
            Lines = null;
        }

        public void Draw(CanvasDrawingSession graphics)
        {
            if(Lines == null)
                return;

            UpdateLines();
            graphics.DrawLine(Lines[0].StartPoint, Lines[0].EndPoint, Colors.White);
            graphics.DrawLine(Lines[1].StartPoint, Lines[1].EndPoint, Colors.White);
            graphics.DrawLine(Lines[2].StartPoint, Lines[2].EndPoint, Colors.White);
        }

        private void UpdateLines()
        {
            if(Lines == null)
                return;


            for(int i = 0; i < Lines.Count; i++)
            {
                Lines[i].StartPoint.X += _deltas[i].X;
                Lines[i].StartPoint.Y += _deltas[i].Y;
                Lines[i].EndPoint.X += _deltas[i].X;
                Lines[i].EndPoint.Y += _deltas[i].Y;
            }
        }
    }

    public class ScoredPointsDisplay
    {
        Vector2 _location;
        Vector2 _startLocation;
        int _points;
        bool _active;
        
        public void Display(Vector2 location, int points)
        {
            _active = true;
            _location = location;
            _points = points;
            _startLocation = _location;
        }

        public void Draw(CanvasDrawingSession graphics)
        {
            if (IsActive())
            {
                string points = string.Format("{0}", _points);
                graphics.DrawText(points, _location, Colors.White);
            }
        }

        public void Update()
        {
            _location = new Vector2(_location.X, _location.Y - 2);
            if (_startLocation.Y - _location.Y > 40)
                _active = false;
        }

        private bool IsActive()
        {
            return _active;
        }
    }
}