using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGuiNet;

namespace Chess;

public class Chess : Game
{
    public static Chess Instance;
    
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private ImGuiRenderer imGuiRenderer;

    public int WindowWidth { get => Window.ClientBounds.Width; init => graphics.PreferredBackBufferWidth = value; }
    public int WindowHeight { get => Window.ClientBounds.Height; init => graphics.PreferredBackBufferHeight = value; }
    public Point WindowSize => Window.ClientBounds.Size;

    private Board board;

    public Chess()
    {
        Instance = this;
        
        graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        WindowWidth = 1600;
        WindowHeight = 900;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        imGuiRenderer = new(this);
        
        board = new();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new(GraphicsDevice);

        Board.PiecesTexture = Content.Load<Texture2D>("pieces.png");

        imGuiRenderer.RebuildFontAtlas();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();
        
        board.Render(spriteBatch);
        
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
