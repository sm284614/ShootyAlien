using AlienShooty.Interface;
using AlienShooty.Stages;
using AlienShooty.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AlienShooty;

// this is when you get when you create a new monogame project.
// the base class just had a loop doing Update, then Draw
public class Game1 : Game
{
    //default stuff
    private GraphicsDeviceManager _graphics; //manages the graphics device
    private SpriteBatch _spriteBatch; //deals with drawing 2D images

    //my stuff
    public static bool DebugMode = false;
    public static GameState State { get; private set; }
    public static Point Resolution { get; private set; }
    private UserInterface _interface;
    private ContentLoader _contentLoader;
    private InputManager _input;
    private Stage _stage;
    private RenderTarget2D _renderTarget;
    private Rectangle _outputRectangle;
    private string _stageFile = "stage1.csv";
    private SpriteFont _debugFont;

    public Game1() // from monogame default project
    {
        State = GameState.LoadingMainMenu;
        _graphics = new GraphicsDeviceManager(this);        
        Content.RootDirectory = "Content"; //content is added through "Content/content.mgcb", a little program that comes with Monogame
        IsMouseVisible = true;
    }
    public enum GameState
    {
        LoadingMainMenu,
        MainMenu,
        LoadingStage,
        InStage,
        Paused,
        Exiting
    }
    public static void ChangeState(GameState newState)
    {
        State = newState;
    }

    protected override void Initialize() // from monogame default project
    {
        base.Initialize();
        _contentLoader = new ContentLoader(Services, _graphics);
        _input = new InputManager();
        _interface = new UserInterface(_contentLoader, _input);
        SetGraphics();
        Debugging.Initialise(_graphics.GraphicsDevice);
    }

    protected override void LoadContent() // from monogame default project
    {
        // I don't really use this bit for much
        _debugFont = Content.Load<SpriteFont>("debugFont");
    }
    // various graphics things we can change
    private void SetGraphics()
    {
        Resolution = new Point(1920, 1080);
        IsMouseVisible = true;
        Window.IsBorderless = true;
        Window.AllowUserResizing = false;
        Window.Title = "ALIEN SHOOTY";
        IsFixedTimeStep = true; //false means unlimited frame rate (BAD, leave as true)
        _graphics.IsFullScreen = false;
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.PreferredBackBufferWidth = Resolution.X;
        _graphics.PreferredBackBufferHeight = Resolution.Y;
        _outputRectangle = new Rectangle(0, 0, Resolution.X, Resolution.Y);
        _renderTarget = new RenderTarget2D(GraphicsDevice, Resolution.X, Resolution.Y);
        _graphics.ApplyChanges();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        //update all this stuff
        _input.Update(gameTime);
        _interface.Update(gameTime);
        if (_input.KeyReleased(Keys.F11)) DebugMode = !DebugMode;
        switch (State)
        {
            case GameState.LoadingMainMenu:
                _interface = new UserInterface(_contentLoader, _input);
                ChangeState(GameState.MainMenu);
                break;
            case GameState.MainMenu:
                _interface.MainMenu.Update(gameTime);
                break;
            case GameState.LoadingStage:
                _stage = new Stage(_stageFile, _contentLoader, _input, _graphics.GraphicsDevice); 
                ChangeState(GameState.InStage);
                break;
            case GameState.InStage:
                _stage.Update(gameTime);
                if (_input.KeyReleased(Keys.Escape))
                {
                    ChangeState(Game1.GameState.MainMenu);
                }
                break;
            case GameState.Paused:                
                _interface.PauseMenu.Update(gameTime);
                break;
            case GameState.Exiting:
                Exit();
                break;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {      
        GraphicsDevice.SetRenderTarget(_renderTarget); //we're drawing everything to a texture
        GraphicsDevice.Clear(Color.Black);

        //I'd probably rather just call two things and handle the menus inside the interface class:
        //_stage?.Draw(spriteBatch);
        //_interface.Draw(spriteBatch);
        switch (State)
        {
            case GameState.LoadingMainMenu:
                //splash screen or something
                break;
            case GameState.MainMenu:
                _interface.MainMenu.Draw(_spriteBatch);
                break;
            case GameState.LoadingStage:
                //loading screen
                break;
            case GameState.InStage:
                _stage.Draw(_spriteBatch);
                break;
            case GameState.Paused:
                _stage.Draw(_spriteBatch);
                _interface.PauseMenu.Draw(_spriteBatch);
                break;
            case GameState.Exiting:
                //fade out
                break;
        }

        GraphicsDevice.SetRenderTarget(null); //rendertarget null draws to the screen

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //pointclamp is nearest neighbour scaling
        _spriteBatch.Draw(_renderTarget, _outputRectangle, Color.White); //draw the texture to the screen area
        _spriteBatch.DrawString(_debugFont, Debugging.DebugText, new Vector2(10, 10), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
