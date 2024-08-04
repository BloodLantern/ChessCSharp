using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chess.Pieces;

public class Pawn : Piece
{
    public override float TextureOffset => TextureSize * 5f;
    public override char FenBaseChar => 'P';

    public int MoveDirection { get; } 
    
    private int StartingY { get; }

    public Pawn(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
        MoveDirection = IsWhite ? -1 : 1;
        StartingY = IsWhite ? 6 : 1;
    }
    
    public override List<Tile> GetReachableTiles()
    {
        List<Tile> tiles = [];

        Point forwardPosition = new(TilePosition.X, TilePosition.Y + MoveDirection);
        Tile forwardTile = Board[forwardPosition];

        if (!forwardTile.HasPiece)
        {
            tiles.Add(forwardTile);

            // Initial 2-tiles move
            if (StartingY == TilePosition.Y)
            {
                Tile forwardForwardTile = Board[forwardPosition.X, forwardPosition.Y + MoveDirection];
                if (!forwardForwardTile.HasPiece)
                    tiles.Add(forwardForwardTile);
            }
        }

        Tile leftTile = Board[forwardPosition.X - 1, forwardPosition.Y];
        if (leftTile is { HasPiece: true } && leftTile.Piece.IsEnemyOf(this))
            tiles.Add(leftTile);
        
        Tile rightTile = Board[forwardPosition.X + 1, forwardPosition.Y];
        if (rightTile is { HasPiece: true } && rightTile.Piece.IsEnemyOf(this))
            tiles.Add(rightTile);

        return tiles;
    }
}
