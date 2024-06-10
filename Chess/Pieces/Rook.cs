namespace Chess.Pieces;

public class Rook : Piece
{
    public override float TextureOffset => TextureSize * 4f;
    public override char FenBaseChar => 'R';

    public Rook(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
}
