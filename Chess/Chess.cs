using Chess.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.ImGuiNet;
using MonoGame.Utils;

namespace Chess;

public class Chess : Game
{
    public static Chess Instance { get; private set; }
    
    public GraphicsDeviceManager Graphics { get; }
    public SpriteBatch SpriteBatch { get; private set; }
    public ImGuiRenderer ImGuiRenderer { get; private set; }

    public FramesPerSecondCounterComponent FpsCounter { get; private set; }

    public int WindowWidth { get => Window.ClientBounds.Width; init => Graphics.PreferredBackBufferWidth = value; }
    public int WindowHeight { get => Window.ClientBounds.Height; init => Graphics.PreferredBackBufferHeight = value; }
    public Point WindowSize => Window.ClientBounds.Size;

    public SpriteFont Font { get; private set; }

    public Board Board { get; private set; }

    public Chess()
    {
        Instance = this;
        
        Graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        WindowWidth = 1600;
        WindowHeight = 900;
        IsFixedTimeStep = false;
        Graphics.SynchronizeWithVerticalRetrace = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        ImGuiRenderer = new(this);
        
        Board = new(this);
        
        Components.Add(FpsCounter = new(this));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new(GraphicsDevice);

        Font = Content.Load<SpriteFont>("font");
        Board.LoadContent(Content);
        Piece.LoadContent(Content);
        Move.LoadContent(Content);

        ImGuiRenderer.RebuildFontAtlas();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardExtended.Refresh();
        MouseExtended.Refresh();

        if (IsActive)
        {
            KeyboardStateExtended keyboard = KeyboardExtended.GetState();
            MouseStateExtended mouse = MouseExtended.GetState();

            Board.Update(keyboard, mouse);
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

        ImGuiRenderer.BeginLayout(gameTime);

        SpriteBatch.Begin();
        
        Board.Draw(SpriteBatch);
        
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
        
        SpriteBatch.DrawString(Font, $"FPS: {FpsCounter.FramesPerSecond}", Vector2.Zero, Color.White);
        
        SpriteBatch.End();

        base.Draw(gameTime);
        
        ImGuiRenderer.EndLayout();
    }
}
