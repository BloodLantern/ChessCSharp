using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Chess;

public class Tile
{
    public const float Size = 64f;

    private static readonly Color WhiteColor = new(0xED, 0xD6, 0xB0);
    private static readonly Color BlackColor = new(0xB8, 0x87, 0x62);
    
    public bool White { get; }
    public bool Black => !White;
    
    public Point Position { get; }

    public Tile(Point position, bool isWhite)
    {
        Position = position;
        White = isWhite;
    }

    public void Render(SpriteBatch spriteBatch, Vector2 offset)
    {
        spriteBatch.DrawRectangle(offset + Position.ToVector2() * Size, new(Size, Size), White ? WhiteColor : BlackColor, Size / 2f);
    }
}
