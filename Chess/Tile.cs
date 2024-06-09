using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Chess;

public class Tile
{
    public const float Size = 110f;

    private static readonly Color WhiteColor = new(0xED, 0xD6, 0xB0);
    private static readonly Color BlackColor = new(0xB8, 0x87, 0x62);
    
    public bool IsWhite { get; }
    public bool IsBlack => !IsWhite;
    
    public Point TilePosition { get; }
    public Vector2 Position => TilePosition.ToVector2() * Size;

    public Tile(Point tilePosition, bool isWhite)
    {
        TilePosition = tilePosition;
        IsWhite = isWhite;
    }

    public void Render(SpriteBatch spriteBatch, Vector2 offset)
        => spriteBatch.DrawRectangle(offset + Position, new(Size, Size), IsWhite ? WhiteColor : BlackColor, Size / 2f);
}
