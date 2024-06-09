namespace Chess.Pieces;

public class Knight : Piece
{
    public override float TextureOffset => TextureSize * 3f;

    public Knight(Tile tile, bool isWhite)
        : base(tile, isWhite)
    {
    }
}
