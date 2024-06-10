namespace Chess.Pieces;

public class Queen : Piece
{
    public override float TextureOffset => TextureSize * 1f;
    public override char FenBaseChar => 'Q';

    public Queen(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
}
