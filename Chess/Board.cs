using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess;

public class Board
{
    public const int Size = 8;

    public static Texture2D PiecesTexture;

    public static Vector2 DrawOffset => Chess.Instance.WindowSize.ToVector2() * 0.5f - Vector2.One * Size * Tile.Size * 0.5f;
    
    public Tile[,] Tiles { get; } = new Tile[Size, Size];

    public Board()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
                Tiles[x, y] = new(new(x, y), (x + y) % 2 == 0);
        }
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in Tiles)
            tile.Render(spriteBatch, DrawOffset);
    }
}
