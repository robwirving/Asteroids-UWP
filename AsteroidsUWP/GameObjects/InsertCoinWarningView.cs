using System.Numerics;
using Windows.UI;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.GameObjects
{
    public class InsertCoinWarningView
    {
        private readonly IGameWindow _parent;

        public InsertCoinWarningView(IGameWindow parent)
        {
            _parent = parent;
        }

        public void Draw(CanvasDrawingSession graphics)
        {
            Draw(graphics, "PRESS S TO START GAME!", 0);
            Draw(graphics, "F to shoot and arrows to move ship", 20);
            Draw(graphics, "X to exit game", 40);
        }

        public void Draw(CanvasDrawingSession graphics, string message, int offset)
        {
            var position = new Vector2((int)(_parent.WindowWidth / 2),
                                       (int)(_parent.WindowHeight / 2 + offset));

            graphics.DrawText(message, position, Colors.White);
        }
    }
}