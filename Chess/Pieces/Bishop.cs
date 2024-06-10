namespace Chess.Pieces;

public class Bishop : Piece
{
    public override float TextureOffset => TextureSize * 2f;
    public override char FenBaseChar => 'B';

    public Bishop(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
}
