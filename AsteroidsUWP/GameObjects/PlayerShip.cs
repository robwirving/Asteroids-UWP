using System;
using System.Numerics;
using Windows.UI;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace AsteroidsUWP.GameObjects
{
    public class PlayerShip : IGameObject
    {
        //private Vector2 _location;
        private IGameWindow _parentWindow;
        private Sprite _shipSprite;
        private bool _isActive = true;
        private int _shieldCountDown = Constants.ShieldCountDown;
        private int _shields = 3;
        private bool _thrusterIsOn;
        private DateTime _shipBlewUp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        private ShipBlowUpScene _blowupScene = new ShipBlowUpScene();

        public PlayerShip(IGameWindow parentWindow)
        {
            _parentWindow = parentWindow;
            _isActive = true;
            _shipSprite = new Sprite {TravelDirectionInDegrees = 30f};
            _shipSprite.Location = new Vector2((float)(_parentWindow.WindowWidth*0.5), (float)(_parentWindow.WindowHeight*0.5));
        }

        public void TurnOnThruster()
        {
            _thrusterIsOn = true;
        }

        public void TurnOffThruster()
        {
            _thrusterIsOn = false;
        }

        public void TurnOnShield()
        {
            if (_shields > 0 && _shieldCountDown <= 0)
            {
                _shields--;
                _shieldCountDown = Constants.ShieldCountDown;
            }
        }

        public Sprite Sprite 
        { 
            get 
            { 
                return _shipSprite; 
            } 
        }

        public void SetInactive()
        {
            if (_shieldCountDown < 1)
            {
                _isActive = false;
            }
        }

        public void Activate()
        {
            _isActive = true;
            _shieldCountDown = Constants.ShieldCountDown;
            _shipSprite.Location = new Vector2((float)(_parentWindow.WindowWidth * 0.5), (float)(_parentWindow.WindowHeight * 0.5));

            _shipSprite.DeltaX = 0;
            _shipSprite.DeltaY = 0;
        }

        public void SetShield()
        {
            if (_shieldCountDown < 1)
            {
                _shieldCountDown = Constants.ShieldCountDown;
            }
        }

        public bool ShieldIsOn
        {
            get { return _shieldCountDown > 0; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public void Draw(CanvasDrawingSession graphics)
        {
            if(DateTime.Now.Subtract(_shipBlewUp).TotalMilliseconds < 1000)
            {
                DrawShipBlowingup(graphics);
            }

            if (!_isActive)
                return;

            double rotation = _shipSprite.DirectionOfSprite.DegreesToRadians();
            
            
            if(DateTime.Now.Subtract(_shipBlewUp).TotalMilliseconds > 1000)
            {
                //CreateShip();
                DrawShip(rotation, graphics);
                DrawThruster(rotation, graphics);
            }
        }

        private void DrawShipBlowingup(CanvasDrawingSession graphics)
        {
            _blowupScene.Draw(graphics);
        }

        private void CreateShip()
        {
            _shipSprite.Polygon = new[] { 
                                            //new Vector2(-12 + _location.X, -8 + _location.Y), 
                                            //new Vector2(-12 + _location.X, 8 + _location.Y), 
                                            //new Vector2(12 + _location.X, _location.Y), 
                                            //new Vector2(-12 + _location.X, -8 + _location.Y) };
                                            new Vector2(-12, -8),
                                            new Vector2(-12, 8),
                                            new Vector2(12, 0),
                                            new Vector2(-12, -8) };
        }

        public void BlowUpShip()
        {
            _shipBlewUp = DateTime.Now;
            _blowupScene.Start(this);
        }

        private void DrawShip(double rotation, CanvasDrawingSession graphics)
        {
            var rotatedPolygon = _shipSprite.Polygon.ClonePolygon().Rotate(new Vector2(0,0), rotation);
            var shipGeometry = CanvasGeometry.CreatePolygon(graphics, rotatedPolygon);

            if (_shieldCountDown > 0)
            {
                graphics.DrawGeometry(shipGeometry, _shipSprite.Location, Colors.DarkGray);
                graphics.DrawEllipse(_shipSprite.Location, 20, 20, Colors.DarkGray);
            }
            else
            {
                graphics.FillGeometry(shipGeometry, _shipSprite.Location, Colors.White);
            }
        }

        private void DrawThruster(double rotation, CanvasDrawingSession graphics)
        {
            if (_thrusterIsOn)
            {
                var location = _shipSprite.Location;
                var thruster = new[]{
                                        new Vector2(-18 + location.X, location.Y),
                                        new Vector2(-10 + location.X, -6 + location.Y), 
                                        new Vector2(-10 + location.X, 6 + location.Y), 
                                        new Vector2(-18 + location.X, location.Y),
                                    };

               
                Vector2[] thrusterClone = thruster.ClonePolygon().Rotate(location, rotation);

                graphics.DrawPolygon(thrusterClone, Colors.DarkGray);
            }
        }

        public void Update()
        {
            _shieldCountDown--;

            if (!_isActive)
                return;

            var location = _shipSprite.Location;
            location.X += (int)_shipSprite.DeltaX;
            location.Y += (int)_shipSprite.DeltaY;

            if (location.X < 0)
                location.X = _parentWindow.WindowWidth;
            if (location.X > _parentWindow.WindowWidth)
                location.X = 0;
            if (location.Y < 0)
                location.Y = _parentWindow.WindowHeight;
            if (location.Y > _parentWindow.WindowHeight)
                location.Y = 0;

            _shipSprite.Location = location;
        }

        public void Rotate(double rotationDelta)
        {
            _shipSprite.DirectionOfSprite = (_shipSprite.DirectionOfSprite + rotationDelta) % 360;

            if (_shipSprite.DirectionOfSprite < 0)
                _shipSprite.DirectionOfSprite += 360;
        }

        public void RotateRight()
        {
            _shipSprite.DirectionOfSprite = (_shipSprite.DirectionOfSprite + 10) % 360;
        }

        public void RotateLeft()
        {
            _shipSprite.DirectionOfSprite = (_shipSprite.DirectionOfSprite - 10);

            if (_shipSprite.DirectionOfSprite < 0)
                _shipSprite.DirectionOfSprite += 360;
        }

        public void SlowDown()
        {
            double dx = Math.Cos(_shipSprite.DirectionOfSprite.DegreesToRadians());
            double dy = Math.Sin(_shipSprite.DirectionOfSprite.DegreesToRadians());
            double limit = 0.8 * 10;

            if (_shipSprite.DeltaX - dx > -limit && _shipSprite.DeltaX - dx < limit)
                _shipSprite.DeltaX -= dx;
            if (_shipSprite.DeltaY - dy > -limit && _shipSprite.DeltaY - dy < limit)
                _shipSprite.DeltaY -= dy;

        }

        public void Thrust()
        {
            double dx = Math.Cos(_shipSprite.DirectionOfSprite.DegreesToRadians());
            double dy = Math.Sin(_shipSprite.DirectionOfSprite.DegreesToRadians());
            double limit = 0.8 * 10;

            if (_shipSprite.DeltaX + dx > -limit && _shipSprite.DeltaX + dx < limit)
                _shipSprite.DeltaX += dx;
            if (_shipSprite.DeltaY + dy > -limit && _shipSprite.DeltaY + dy < limit)
                _shipSprite.DeltaY += dy;

        }

        public Vector2 Location
        {
            get { return _shipSprite.Location; }
        }

        public double ShipDirection
        {
            get { return _shipSprite.DirectionOfSprite; }
        }

        internal bool IsPointWithin(Vector2 point)
        {
            return _shipSprite.IsPointWithin(point);
        }

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            CreateShip();
            _shipSprite.PolygonGeometry = CanvasGeometry.CreatePolygon(resourceCreator, _shipSprite.Polygon);
        }
    }
}