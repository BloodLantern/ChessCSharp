using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.ImGuiNet;
using MonoGame.Utils;

namespace Chess;

public class Chess : Game
{
    public static Chess Instance;
    
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private ImGuiRenderer imGuiRenderer;

    private FramesPerSecondCounterComponent fpsCounter;

    public int WindowWidth { get => Window.ClientBounds.Width; init => graphics.PreferredBackBufferWidth = value; }
    public int WindowHeight { get => Window.ClientBounds.Height; init => graphics.PreferredBackBufferHeight = value; }
    public Point WindowSize => Window.ClientBounds.Size;

    private SpriteFont font;

    private Board board;

    public Chess()
    {
        Instance = this;
        
        graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        WindowWidth = 1600;
        WindowHeight = 900;
        IsFixedTimeStep = false;
        graphics.SynchronizeWithVerticalRetrace = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        imGuiRenderer = new(this);
        
        board = new();
        
        Components.Add(fpsCounter = new(this));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new(GraphicsDevice);

        font = Content.Load<SpriteFont>("font");
        Board.PiecesTexture = Content.Load<Texture2D>("pieces");

        imGuiRenderer.RebuildFontAtlas();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardExtended.Refresh();
        MouseExtended.Refresh();

        if (IsActive)
        {
            KeyboardStateExtended keyboard = KeyboardExtended.GetState();
            MouseStateExtended mouse = MouseExtended.GetState();

            board.Update(keyboard, mouse);
        }
        
        foreach (IGameComponent component in Components)
        {
            switch (component)
            {
                case IUpdate update:
                    update.Update(gameTime);
                    break;
                case IUpdateable updateable:
                    updateable.Update(gameTime);
                    break;
            }
        }
        
        Coroutine.UpdateAll(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        imGuiRenderer.BeginLayout(gameTime);

        spriteBatch.Begin();
        
        board.Draw(spriteBatch);
        
        foreach (IGameComponent component in Components)
        {
            switch (component)
            {
                case DrawableGameComponent drawableComponent:
                    drawableComponent.Draw(gameTime);
                    break;
                case IDrawable drawable:
                    drawable.Draw(gameTime);
                    break;
            }
        }
        
        spriteBatch.DrawString(font, $"FPS: {fpsCounter.FramesPerSecond}", Vector2.Zero, Color.White);
        
        spriteBatch.End();

        base.Draw(gameTime);
        
        imGuiRenderer.EndLayout();
    }
}
