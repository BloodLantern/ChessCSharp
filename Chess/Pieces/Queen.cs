namespace Chess.Pieces;

public class Queen : Piece
{
    public override float TextureOffset => TextureSize * 1f;
    public override char FenBaseChar => 'Q';

    public Queen(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
    
    public override void UpdateReachableTiles()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                
                int offset = 1;
                Tile tile = Board[TilePosition.X + x, TilePosition.Y + y];

                while (tile != null)
                {
                    if (tile.HasPiece)
                    {
                        if (tile.Piece.IsEnemyOf(this))
                            ReachableTiles.Add(tile);
                        break;
                    }
                    ReachableTiles.Add(tile);
                    
                    offset++;
                    tile = Board[TilePosition.X + x * offset, TilePosition.Y + y * offset];
                }
            }
        }
    }
}
