using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.GameObjects
{
    public class Photon : IGameObject
    {
        private float _maxDistance;
        private float _distanceTraveled = Constants.MaxEnemyBulletDistance + 1;
        protected Sprite _bulletSprite = new Sprite();
        protected Vector2 _location = new Vector2(0, 0);
        private PhotonTimeManager _photonTimeManager;
        private IGameWindow _parent;

        public Photon(IGameWindow parent)
        {
            _parent = parent;
        }

        public void Fire(Vector2 startLocation, double direction, float maxDistance, PhotonTimeManager photoTimeManager)
        {
            _photonTimeManager = photoTimeManager;

            if(!EnoughTimeHasPassed())
                return;

            _maxDistance = maxDistance;
            _distanceTraveled = 0;
            _bulletSprite = new Sprite();
            _bulletSprite.Polygon = new[]
                                        {
                                            new Vector2((float)startLocation.X, (float)startLocation.Y),
                                            new Vector2((float)startLocation.X+1, (float)startLocation.Y+1),
                                            new Vector2((float)startLocation.X, (float)startLocation.Y)
                                        };

            _bulletSprite.Speed = Constants.BulletSpeed;
            _bulletSprite.TravelDirectionInDegrees = direction;
            _location = startLocation;
            _photonTimeManager.SetFired();
            
        }

        private bool EnoughTimeHasPassed()
        {
            return _photonTimeManager.EnoughTimeHasPassed();
        }

        public Vector2 Location
        {
            get { return _location; }
        }

        public bool IsActive
        {
            get
            {
                return _distanceTraveled < _maxDistance;
            }
        }

        public void Update()
        {
            if (_bulletSprite != null && IsActive)
            {
                double radians = _bulletSprite.TravelDirectionInDegrees.DegreesToRadians();

                _location.X += (int)(_bulletSprite.Speed * Math.Cos(radians));
                _location.Y += (int)(_bulletSprite.Speed * Math.Sin(radians));

                if (_location.X < 0)
                    _location.X = _parent.WindowWidth;
                if (_location.X > _parent.WindowWidth)
                    _location.X = 0;
                if (_location.Y < 0)
                    _location.Y = _parent.WindowHeight;
                if (_location.Y > _parent.WindowHeight)
                    _location.Y = 0;

                _distanceTraveled += _bulletSprite.Speed;
            }
        }
        
        public virtual void Draw(CanvasDrawingSession graphics)
        {
            if(_bulletSprite != null && IsActive)
            {
                _bulletSprite.Polygon = new Vector2[] { new Vector2(-1 + _location.X, -1 + _location.Y), new Vector2(-1 + _location.X, 1 + _location.Y), new Vector2(1 + _location.X, _location.Y), new Vector2(-1 + _location.X, -1 + _location.Y) };

                var clone = _bulletSprite.Polygon.ClonePolygon();

                graphics.DrawPolygon(clone, Colors.White);
            }
        }

        public void SetInactive()
        {
            _distanceTraveled = _maxDistance + 1;
        }
    }
}