namespace Chess.Pieces;

public class Pawn : Piece
{
    public override float TextureOffset => TextureSize * 5f;

    public Pawn(Tile tile, bool isWhite)
        : base(tile, isWhite)
    {
    }
}
