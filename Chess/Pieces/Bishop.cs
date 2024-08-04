﻿using System.Collections.Generic;

namespace Chess.Pieces;

public class Bishop : Piece
{
    public override float TextureOffset => TextureSize * 2f;
    public override char FenBaseChar => 'B';

    public Bishop(Board board, Tile tile, bool isWhite)
        : base(board, tile, isWhite)
    {
    }
    
    public override List<Tile> GetReachableTiles()
    {
        List<Tile> tiles = [];
        
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
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
        
        return tiles;
    }
}
