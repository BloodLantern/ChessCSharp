using System.Collections.Generic;

namespace Chess.Pieces;

public class Rook : Piece
{
    public override float TextureOffset => TextureSize * 4f;
    public override char FenBaseChar => 'R';

    public Rook(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
    
    public override List<Tile> GetReachableTiles()
    {
        List<Tile> tiles = [];

        for (int x = -1; x <= 1; x += 2)
            GetReachableTiles(tiles, x, 0);
        
        for (int y = -1; y <= 1; y += 2)
            GetReachableTiles(tiles, 0, y);

        return tiles;
    }

    private void GetReachableTiles(List<Tile> tiles, int x, int y)
    {
        int offset = 1;
        Tile tile = Board[TilePosition.X + x, TilePosition.Y + y];

        while (tile != null)
        {
            if (tile.HasPiece)
            {
                if (tile.Piece.IsEnemyOf(this))
                    tiles.Add(tile);
                break;
            }
            tiles.Add(tile);
                    
            offset++;
            tile = Board[TilePosition.X + x * offset, TilePosition.Y + y * offset];
        }
    }
}
