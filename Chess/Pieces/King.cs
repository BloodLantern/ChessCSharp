namespace Chess.Pieces;

public class King : Piece
{
    public override float TextureOffset => TextureSize * 0f;
    public override char FenBaseChar => 'K';

    public King(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
}
