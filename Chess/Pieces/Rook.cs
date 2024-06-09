namespace Chess.Pieces;

public class Rook : Piece
{
    public override float TextureOffset => TextureSize * 4f;

    public Rook(Tile tile, bool isWhite)
        : base(tile, isWhite)
    {
    }
}
