namespace Chess.Pieces;

public class King : Piece
{
    public override float TextureOffset => TextureSize * 0f;

    public King(Tile tile, bool isWhite)
        : base(tile, isWhite)
    {
    }
}
