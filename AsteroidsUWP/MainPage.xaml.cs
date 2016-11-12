using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AsteroidsUWP.Core;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AsteroidsUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IGameWindow, IGameController
    {
        private RadialController _controller;
        private AsteroidsGame _game;
        private bool _gameStarted;
        private readonly RunAloneGameController _gameController = new RunAloneGameController();
        private Size _windowSize;
        private RadialControllerMenuItem _radialMenuItem;

        public MainPage()
        {
            InitializeComponent();

            MouseState = new MouseState
            {
                IsMouseButtonDown = false,
                MousePosition = new Point(-1, -1)
            };

            KeyboardState = new KeyboardState
            {
                IsLeftKeyDown = false,
                IsRightKeyDown = false
            };

            RadialControllerState = new RadialControllerState
            {
                IsButtonPressed = false,
                RotationDelta = 0
            };

            InitializeController();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _controller?.Menu.Items.Clear();

            base.OnNavigatedFrom(e);
        }

        private void InitializeController()
        {
            _controller = RadialController.CreateForCurrentView();
            _controller.RotationResolutionInDegrees = 5;

            // Wire events
            _controller.RotationChanged += Controller_RotationChanged;
            _controller.ButtonClicked += Controller_ButtonClicked;

            _controller.Menu.Items.Clear();
            _radialMenuItem = RadialControllerMenuItem.CreateFromKnownIcon("Asteroids", RadialControllerMenuKnownIcon.Scroll);
            _controller.Menu.Items.Add(_radialMenuItem);
            _controller.Menu.SelectMenuItem(_radialMenuItem);
            //_controller.Menu.IsEnabled = true;
        }

        private void Controller_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            RadialControllerState.RotationDelta = args.RotationDeltaInDegrees;
        }

        private void Controller_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            if (!_gameStarted)
                StartNewGame(this);
            else
            {
                RadialControllerState.IsButtonPressed = true;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWindowSize();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWindowSize();
        }

        private void UpdateWindowSize()
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            //var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            _windowSize = new Size(bounds.Width, bounds.Height);
        }
        private void OnCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            _game.CreateResources(sender);
        }

        private void OnGameLoopStarting(ICanvasAnimatedControl sender, object args)
        {
            _game = new AsteroidsGame(this, _gameController);

            GameManager.CreateTheGame(_game);

            _game.Run();
        }

        private void OnDraw(ICanvasAnimatedControl canvasAnimatedControl, CanvasAnimatedDrawEventArgs args)
        {
            _game.Draw(args.DrawingSession);
        }

        private void OnUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            _game.Update();
        }

        private void StartNewGame(IGameController gameController)
        {
            _gameStarted = true;
            GameManager.TheGameManager.ReInitializeGame(gameController);
        }

        public int WindowWidth { get { return (int)_windowSize.Width; } }
        public int WindowHeight { get { return (int)_windowSize.Height; } }
        public MouseState MouseState { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public RadialControllerState RadialControllerState { get; set; }
        public bool Standalone { get; set; }
    }
}
