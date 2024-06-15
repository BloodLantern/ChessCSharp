namespace Chess.Pieces;

public class King : Piece
{
    public override float TextureOffset => TextureSize * 0f;
    public override char FenBaseChar => 'K';

    public King(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
    
    public override void UpdateReachableTiles()
    {
        for (int x = -1; x < 1; x++)
        {
            for (int y = -1; y < 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Tile tile = Board[TilePosition.X + x, TilePosition.Y + y];
                if (tile != null && (!tile.HasPiece || tile.Piece.IsEnemyOf(this)))
                    ReachableTiles.Add(tile);
            }
        }
    }
}
