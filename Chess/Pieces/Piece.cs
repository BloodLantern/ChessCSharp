using Microsoft.Xna.Framework.Graphics;

namespace Chess.Pieces;

public abstract class Piece
{
    public abstract Texture2D WhiteTexture { get; }
    public abstract Texture2D BlackTexture { get; }
}
