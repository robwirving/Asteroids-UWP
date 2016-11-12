using System.Numerics;
using Windows.UI;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

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
            Draw(graphics, "PRESS DIAL BUTTON TO START GAME!", 0);
            Draw(graphics, "Press dial to shoot and rotate dial to aim", 20);
        }

        public void Draw(CanvasDrawingSession graphics, string message, int offset)
        {
            var position = new Vector2((int)(_parent.WindowWidth / 2),
                                       (int)(_parent.WindowHeight / 2 + offset));

            var format = new CanvasTextFormat {HorizontalAlignment = CanvasHorizontalAlignment.Center};
            graphics.DrawText(message, position, Colors.White, format);
        }
    }
}