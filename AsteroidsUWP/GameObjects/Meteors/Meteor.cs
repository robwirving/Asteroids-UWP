using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.GameObjects.Meteors
{
    public class Meteor : IGameObject
    {
        private Sprite _meteorSprite;
        private Vector2 _location = new Vector2(50, 50);
        private Vector2[] _originalMeteorPolygon;
        private double _rotationDelta = 2;
        private double _rotation = 10;
        private MeteorType _meteorType;
        private bool _isActive;
        private IGameWindow _parent;

        //Services
        private static Random _randomGenerator = new Random();

        public bool IsActive 
        { 
            get
            { 
                return _isActive && !(_meteorType is NullMeteorType); 
            }
            set
            {
                _isActive = value;
            }
        }

        public int Score
        {
            get { return _meteorType.Score; }
        }


        public Vector2 Location
        {
            get { return _location; }
        }

        public MeteorType GetNextSmallerMeteor()
        {
            return _meteorType.GetNextSmallerSize();
        }

        public Meteor(MeteorType meteorType, Vector2 initialLocation, IGameWindow parent)
        {
            _parent = parent;
            IsActive = true;
            _meteorSprite = new Sprite();
            _meteorType = meteorType;
            CreateMeteor(initialLocation);
        }

        public void Draw(CanvasDrawingSession graphics)
        {
            if (IsActive)
            {
                double currentRotation = _rotation.DegreesToRadians();

                Vector2[] clone = _meteorSprite.Polygon.ClonePolygon().Rotate(_location, currentRotation);

                //graphics.DrawLine(Pens.White, (float)dx1, (float)dy1, (float)dx2, (float)dy2);
                graphics.DrawPolygon(clone, Colors.White);

                //graphics.DrawString(_meteorType.Text, new Font("Courier New", 10), Brushes.White, _location.X, _location.Y);
            }
        }

        public void Update()
        {
            if(_meteorSprite == null || !IsActive)
                return;

            UpdateTheLocation();
            ChangeLocationToOppositeSideOfCanvas();
            UpdateTheRotation();
            UpdateMeteorPolygonPoints();
        }

        public void TestIfShot(Photon photon)
        {
            if (IsActive && photon.IsActive && _meteorSprite.IsPointWithin(photon.Location))
            {
                IsActive = false;
                photon.SetInactive();

                if(!(_meteorType is NullMeteorType))
                {
                    GameManager.RaiseMeteorHit(this);
                }
            }
        }

        public bool IsPointWithin(Vector2 point)
        {
            return _meteorSprite.IsPointWithin(point);
        }

        public bool CollidesWithShip(PlayerShip ship)
        {
             if (IsActive && 
                 ship.IsActive &&
                 !ship.ShieldIsOn &&
                 _meteorSprite.CollidesWith( ship.Sprite ))
                 return true;

            return false;
        }

        private void UpdateTheLocation()
        {
            _location.X += (int)_meteorSprite.DeltaX;
            _location.Y += (int)_meteorSprite.DeltaY;
        }

        private void UpdateTheRotation()
        {
            _rotation += _rotationDelta;

            if (_rotationDelta > 360)
                _rotationDelta -= 360;
            if (_rotationDelta < 0)
                _rotationDelta += 360;
        }

        private void ChangeLocationToOppositeSideOfCanvas()
        {
            int outerScreenMargin = 50;

            if (_location.X < -outerScreenMargin)
                _location.X = _parent.WindowWidth + outerScreenMargin;
            if (_location.X > _parent.WindowWidth + outerScreenMargin)
                _location.X = -outerScreenMargin;
            if (_location.Y < -outerScreenMargin)
                _location.Y = _parent.WindowHeight + outerScreenMargin;
            if (_location.Y > _parent.WindowHeight + outerScreenMargin)
                _location.Y = -outerScreenMargin;
        }

        private void UpdateMeteorPolygonPoints()
        {
            Vector2[] meteor = new Vector2[_meteorSprite.Polygon.Length];

            for(int i = 0; i  < _meteorSprite.Polygon.Length; i++)
                meteor[i] = new Vector2(_location.X + _originalMeteorPolygon[i].X, _location.Y + _originalMeteorPolygon[i].Y);
 
            _meteorSprite.Polygon = meteor;
        }

        private void CreateMeteor(Vector2 initialLocation)
        {
            CreateMeteorPolygon();
            SetMeteorSpritesPolygon();
            SetRandomVelocity();
            SetRandomRotation();

            if (initialLocation.X == 0 && initialLocation.Y == 0)
                SetRandomLocationOnTheScreen();    
            else
                _location = SpreadOut(initialLocation);
            
        }

        private Vector2 SpreadOut(Vector2 location)
        {
            location.X += _randomGenerator.Next(20) - 10;
            location.Y += _randomGenerator.Next(20)  - 10;

            return location;
        }

        private void SetMeteorSpritesPolygon()
        {
            _meteorSprite.Polygon = _originalMeteorPolygon.ClonePolygon();
        }

        private void SetRandomLocationOnTheScreen()
        {
            _location = new Vector2(_randomGenerator.Next(_parent.WindowWidth), _randomGenerator.Next(_parent.WindowHeight));
        }

        private void SetRandomRotation()
        {
            _rotationDelta = _randomGenerator.Next(10) - 5;
        }

        private void SetRandomVelocity()
        {
            int speed = _randomGenerator.Next(3)+5;

            int directionOfTravel = _randomGenerator.Next(360);

            _meteorSprite.DeltaX = speed * Math.Cos(MathHelper.DegreesToRadians(directionOfTravel));
            _meteorSprite.DeltaY = speed * Math.Sin(MathHelper.DegreesToRadians(directionOfTravel));
        }

        private void CreateMeteorPolygon()
        {
            Vector2 location = new Vector2(0, 0);
            int points = _meteorType.MinimumNumberOfPoints + _meteorType.MinimumNumberOfPoints/2;
            float runningAngle = 0;
            var polygon = new List<Vector2>();

            while(runningAngle < 300)
            {
                float randomRadius = _meteorType.Radius +_randomGenerator.Next((int)(_meteorType.Radius / 2));
                float subsequeRandomAngle = _randomGenerator.Next(600 / points);

                runningAngle += subsequeRandomAngle;
                double radianAngle = MathHelper.DegreesToRadians(runningAngle);

                polygon.Add(new Vector2(location.X + (int)(randomRadius * Math.Cos(radianAngle)),
                                                      location.Y + (int)(randomRadius * Math.Sin(radianAngle))));
            }

            _originalMeteorPolygon = polygon.ToArray();
        }
    }
}