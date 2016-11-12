using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.Core
{
    public interface IBoardDrawingMachine
    {
        void DrawBoard(CanvasDrawingSession formGraphics, Size boardSize);
    }
}