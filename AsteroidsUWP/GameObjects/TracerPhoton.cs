using System.Numerics;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.GameObjects
{
    public class TracerPhoton : Photon
    {
        public TracerPhoton(IGameWindow parent) : base(parent)
        {
            
        }

        public override void Draw(CanvasDrawingSession graphics)
        {
            if (_bulletSprite != null && IsActive)
            {
                _bulletSprite.Polygon = new Vector2[] { new Vector2(-1 + _location.X, -1 + _location.Y), new Vector2(-1 + _location.X, 1 + _location.Y), new Vector2(1 + _location.X, _location.Y), new Vector2(-1 + _location.X, -1 + _location.Y) };

                var clone = _bulletSprite.Polygon.ClonePolygon();

                //graphics.DrawPolygon(Pens.Red, clone);
            }
        }
    }
}