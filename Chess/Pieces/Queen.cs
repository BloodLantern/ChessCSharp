namespace Chess.Pieces;

public class Queen : Piece
{
    public override float TextureOffset => TextureSize * 1f;

    public Queen(Tile tile, bool isWhite)
        : base(tile, isWhite)
    {
    }
}
