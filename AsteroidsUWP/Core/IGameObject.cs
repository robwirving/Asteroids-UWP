using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.Core
{
    public interface IGameObject
    {
        void Update();
        void Draw(CanvasDrawingSession graphics);
    }
}
