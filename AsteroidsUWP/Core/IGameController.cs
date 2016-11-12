namespace AsteroidsUWP.Core
{
    public interface IGameController
    {
        MouseState MouseState { get; set; }
        KeyboardState KeyboardState{ get; set; }
        RadialControllerState RadialControllerState { get; set; }
        bool Standalone { get; set; }
    }
}