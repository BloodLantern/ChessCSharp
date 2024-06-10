namespace Chess.Pieces;

public class Pawn : Piece
{
    public override float TextureOffset => TextureSize * 5f;
    public override char FenBaseChar => 'P';

    public Pawn(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
}
