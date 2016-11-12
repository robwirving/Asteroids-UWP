using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.GameObjects
{
    public class EnemyShip : IGameObject
    {
        Vector2 _location = new Vector2(-40, -40);
        Sprite _shipSprite;
        private DateTime _startTime;
        private float _deltaX = -3;
        private float _deltaY;
        readonly Random _random = new Random();
        private IGameWindow _parent;

        public EnemyShip(IGameWindow parent)
        {
            _parent = parent;
            _location = new Vector2(-40, -40);
        }

        public Vector2 Location
        {
            get
            {
                return _location;
            }
        }

        public void Activate()
        {
            _shipSprite = new Sprite();
            _shipSprite.Polygon = new[]
            {
                new Vector2(0, 0), 
                new Vector2(30, 0), 
                new Vector2(30, 20), 
                new Vector2(0, 20), 
                new Vector2(0, 0)};

            _startTime = DateTime.Now;
            _location = new Vector2(_parent.WindowWidth, 60);
        }

        public bool IsActive
        {
            get
            {
                return _location.X > 0;
            }
        }

        public void Update()
        {

            if (DateTime.Now.Subtract(_startTime).TotalMilliseconds > 750)
            {
                int randomValue = _random.Next(3);

                if (randomValue % 2 == 0)
                {
                    _deltaY = 3;
                }
                else if(randomValue % 3 == 0)
                {
                    _deltaY = -3;
                }
                
                _startTime = DateTime.Now;
            }

            _location.X += (int)_deltaX;
            _location.Y += (int)_deltaY;

            if (_location.Y < 0)
                _deltaY = 3;
        }

        public void Draw(CanvasDrawingSession graphics)
        {
            if (!IsActive)
                return;

            CreateShip();
            DrawShip(graphics);
        }

        private void DrawShip(CanvasDrawingSession graphics)
        {
            //graphics.DrawPolygon(Pens.White, _shipSprite.Polygon
            graphics.DrawLine(new Vector2(_location.X - 20, _location.Y), new Vector2((_location.X+ 20), _location.Y), Colors.White);
            //graphics.DrawLine(Pens.White, _location.X - 20, _location.Y, _location.X + 20, _location.Y);
        }

        private void CreateShip()
        {
            _shipSprite.Polygon = new[] 
                                      { 
                                          new Vector2(_location.X - 5, _location.Y-10), 
                                          new Vector2(_location.X+5, _location.Y-10),
                                          new Vector2(_location.X+15, _location.Y),
                                          new Vector2(_location.X+5, _location.Y+10), 
                                          new Vector2(_location.X-5, _location.Y+10),
                                          new Vector2(_location.X-15, _location.Y),
                                          new Vector2(_location.X-5, _location.Y-10)
                                      };
        }

        internal bool IsPointWithin(Vector2 point)
        {
            return _shipSprite.IsPointWithin(point);
        }

        internal void Inactivate()
        {
            _location.X = -100;
        }
    }
}
