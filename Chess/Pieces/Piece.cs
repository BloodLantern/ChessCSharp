using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Chess.Pieces;

public abstract class Piece
{
    protected const float TextureSize = 150f;
    private const float TextureScale = 1f / TextureSize * Tile.Size;
    
    public abstract float TextureOffset { get; }
    
    public bool IsWhite { get; }
    public bool IsBlack => !IsWhite;

    private Tile tile;
    public Tile Tile
    {
        get => tile;
        set
        {
            tile = value;
            Position = value.Position + Vector2.One * Tile.Size * 0.5f;
        }
    }

    public Vector2 Position { get; set; }

    protected Piece(Tile tile, bool isWhite)
    {
        Tile = tile;
        IsWhite = isWhite;
    }

    public void Render(SpriteBatch spriteBatch, Vector2 offset)
        => spriteBatch.Draw(
            Board.PiecesTexture,
            offset + Position,
            new RectangleF(TextureOffset, IsWhite ? 0f : TextureSize, TextureSize, TextureSize).ToRectangle(),
            Color.White,
            0f,
            Vector2.One * TextureSize * 0.5f,
            TextureScale,
            SpriteEffects.None,
            0f
        );
}
