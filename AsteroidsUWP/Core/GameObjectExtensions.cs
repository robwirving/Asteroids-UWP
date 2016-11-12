using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.Core
{
    public static class GameObjectExtensions
    {
        public static void Update<T>(this IEnumerable<T> objects) where T : IGameObject
        {
            foreach (IGameObject gameObject in objects)
                gameObject.Update();
        }

        public static void Draw<T>(this IEnumerable<T> objects, CanvasDrawingSession graphics) where T : IGameObject
        {
            foreach (IGameObject gameObject in objects)
                gameObject.Draw(graphics);
        }
    }
}
