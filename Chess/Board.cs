using System.Collections.Generic;
using Chess.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess;

public class Board
{
    public const int Size = 8;

    public static Texture2D PiecesTexture;

    public static Vector2 DrawOffset => Chess.Instance.WindowSize.ToVector2() * 0.5f - Vector2.One * Size * Tile.Size * 0.5f;
    
    public Tile[,] Tiles { get; } = new Tile[Size, Size];

    public List<Piece> Pieces { get; } = [];

    public Board()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
                Tiles[x, y] = new(new(x, y), (x + y) % 2 == 0);
        }

        for (int i = 0; i < 2; i++)
        {
            bool black = i == 0;
            
            Pieces.Add(new Rook(Tiles[0, black ? 0 : 7], !black));
            Pieces.Add(new Knight(Tiles[1, black ? 0 : 7], !black));
            Pieces.Add(new Bishop(Tiles[2, black ? 0 : 7], !black));
            Pieces.Add(new Queen(Tiles[3, black ? 0 : 7], !black));
            Pieces.Add(new King(Tiles[4, black ? 0 : 7], !black));
            Pieces.Add(new Bishop(Tiles[5, black ? 0 : 7], !black));
            Pieces.Add(new Knight(Tiles[6, black ? 0 : 7], !black));
            Pieces.Add(new Rook(Tiles[7, black ? 0 : 7], !black));
            
            for (int j = 0; j < Size; j++)
                Pieces.Add(new Pawn(Tiles[j, black ? 1 : 6], !black));
        }
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in Tiles)
            tile.Render(spriteBatch, DrawOffset);

        foreach (Piece piece in Pieces)
            piece.Render(spriteBatch, DrawOffset);
    }
}
