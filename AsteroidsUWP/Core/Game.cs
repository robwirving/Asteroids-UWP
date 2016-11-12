using System;
using System.Threading;
using Microsoft.Graphics.Canvas;

namespace AsteroidsUWP.Core
{
    public class Game
    {
        private readonly IGameWindow _parentWindow;
        private readonly GameStatus _status;
       
        public Game(IGameWindow parent)
        {
            _status = new GameStatus {GameOver = false};
            _parentWindow = parent;
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(CanvasDrawingSession drawingSession)
        {
        }

        public virtual void Run()
        {
        }

        protected IGameWindow ParentWindow
        {
            get
            {
                return _parentWindow;
            }
        }
    }
}