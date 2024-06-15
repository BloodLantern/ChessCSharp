namespace Chess.Pieces;

public class Rook : Piece
{
    public override float TextureOffset => TextureSize * 4f;
    public override char FenBaseChar => 'R';

    public Rook(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
    
    public override void UpdateReachableTiles()
    {
        for (int x = -1; x < 1; x += 2)
            UpdateReachableTiles(x, 0);
        
        for (int y = -1; y < 1; y += 2)
            UpdateReachableTiles(0, y);
    }

    private void UpdateReachableTiles(int x, int y)
    {
        int offset = 0;
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
                    
            tile = Board[TilePosition.X + x * ++offset, TilePosition.Y + y * ++offset];
        }
    }
}
