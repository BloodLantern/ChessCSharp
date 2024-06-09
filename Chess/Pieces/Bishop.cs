namespace Chess.Pieces;

public class Bishop : Piece
{
    public override float TextureOffset => TextureSize * 2f;

    public Bishop(Tile tile, bool isWhite)
        : base(tile, isWhite)
    {
    }
}
