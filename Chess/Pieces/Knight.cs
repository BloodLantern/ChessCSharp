using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chess.Pieces;

public class Knight : Piece
{
    public override float TextureOffset => TextureSize * 3f;
    public override char FenBaseChar => 'N';

    public Knight(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
    
    public override List<Tile> GetReachableTiles()
    {
        Point up = new(TilePosition.X, TilePosition.Y - 2);
        Point down = new(TilePosition.X, TilePosition.Y + 2);
        Point left = new(TilePosition.X - 2, TilePosition.Y);
        Point right = new(TilePosition.X + 2, TilePosition.Y);

        Tile[] tiles =
        [
            Board[up.X - 1, up.Y], // Up-left
            Board[up.X + 1, up.Y], // Up-right
            Board[down.X - 1, down.Y], // Down-left
            Board[down.X + 1, down.Y], // Down-right
            Board[left.X, left.Y - 1], // Left-up
            Board[left.X, left.Y + 1], // Left-down
            Board[right.X, right.Y - 1], // Right-up
            Board[right.X, right.Y + 1], // Right-down
        ];
        
        List<Tile> result = [];

        foreach (Tile tile in tiles)
        {
            if (tile == null)
                continue;
            
            if (!tile.HasPiece || tile.Piece.IsEnemyOf(this))
                result.Add(tile);
        }

        return result;
    }
}
