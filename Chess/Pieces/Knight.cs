namespace Chess.Pieces;

public class Knight : Piece
{
    public override float TextureOffset => TextureSize * 3f;
    public override char FenBaseChar => 'N';

    public Knight(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
}
