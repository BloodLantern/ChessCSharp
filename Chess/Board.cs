using System.Collections.Generic;
using Chess.Pieces;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Chess;

public class Board
{
    public const int TileSize = 8;
    public const int FirstTileIndex = 0;
    public const int LastTileIndex = TileSize - 1;
    public const float Size = Tile.Size * TileSize;

    private static Texture2D Texture { get; set; }
    private static Texture2D ReachableTileTexture { get; set; }
    private static Texture2D ReachableTileEnemyTexture { get; set; }

    public static Vector2 DrawOffset => Chess.Instance.WindowSize.ToVector2() * 0.5f - Vector2.One * Size * 0.5f;
    
    public Tile[,] Tiles { get; } = new Tile[TileSize, TileSize];
    public List<Piece> Pieces { get; } = [];
    public MoveList Moves { get; } = [];
    
    public Piece SelectedPiece { get; set; }
    
    public static RectangleF Area => new(Vector2.Zero, Vector2.One * Size);
    public static RectangleF ScreenArea
    {
        get
        {
            RectangleF area = Area;
            area.Position += DrawOffset;
            return area;
        }
    }

    public bool IsWhiteTurn { get; private set; } = true;

    private static readonly Color ReachableTileColor = new(Color.Black, 0.15f);
    
    public King WhiteKing { get; }
    public King BlackKing { get; }

    public Board()
    {
        for (int x = 0; x < TileSize; x++)
        {
            for (int y = 0; y < TileSize; y++)
                Tiles[x, y] = new(this, new(x, y), (x + y) % 2 == 0);
        }

        for (int i = 0; i < 2; i++)
        {
            bool black = i == 0;
            int y = black ? FirstTileIndex : LastTileIndex;
            
            Pieces.Add(new Rook(this, Tiles[FirstTileIndex + 0, y], !black));
            Pieces.Add(new Knight(this, Tiles[FirstTileIndex + 1, y], !black));
            Pieces.Add(new Bishop(this, Tiles[FirstTileIndex + 2, y], !black));
            Pieces.Add(new Queen(this, Tiles[FirstTileIndex + 3, y], !black));
            
            King king = new(this, Tiles[LastTileIndex - 3, y], !black);
            if (black)
                BlackKing = king;
            else
                WhiteKing = king;
            Pieces.Add(king);
            
            Pieces.Add(new Bishop(this, Tiles[LastTileIndex - 2, y], !black));
            Pieces.Add(new Knight(this, Tiles[LastTileIndex - 1, y], !black));
            Pieces.Add(new Rook(this, Tiles[LastTileIndex - 0, y], !black));
            
            for (int j = 0; j < TileSize; j++)
                Pieces.Add(new Pawn(this, Tiles[j, black ? FirstTileIndex + 1 : LastTileIndex - 1], !black));
        }

        UpdatePiecesReachableTiles();

        Moves.OnMoveAdded += _ =>
        {
            UpdatePiecesReachableTiles();
            IsWhiteTurn = !IsWhiteTurn;
        };
    }

    internal static void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("board");
        ReachableTileTexture = content.Load<Texture2D>("reachable_tile");
        ReachableTileEnemyTexture = content.Load<Texture2D>("reachable_tile_enemy");
    }

    public void Update(KeyboardStateExtended keyboard, MouseStateExtended mouse)
    {
        foreach (Tile tile in Tiles)
            tile.Update(keyboard, mouse);

        if (Moves.CurrentIsLastMove)
        {
            foreach (Piece piece in Pieces)
                piece.Update(mouse);
        }

        if (SelectedPiece != null)
        {
            SelectedPiece.Position = mouse.Position.ToVector2() - DrawOffset;

            if (mouse.IsButtonReleased(MouseButton.Left))
            {
                Vector2 position = SelectedPiece.Position;
                if (!Area.Contains(position))
                {
                    SelectedPiece.ResetPosition();
                }
                else
                {
                    Point tilePosition = (position / Tile.Size).ToPoint();
                    Tile tile = Tiles[tilePosition.X, tilePosition.Y];

                    if (SelectedPiece.Tile != tile && SelectedPiece.ReachableTiles.Contains(tile))
                    {
                        Move move = new(this, SelectedPiece, tile);
                        move.Make(false, true);
                        Moves.Add(move);
                    }
                    else
                    {
                        SelectedPiece.ResetPosition();
                    }
                }

                SelectedPiece = null;
            }
        }

        if (keyboard.IsKeyPressed(Keys.Left))
            Moves.UnmakeOne();
        else if (keyboard.IsKeyPressed(Keys.Right))
            Moves.MakeOne();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, DrawOffset, Color.White);
        
        foreach (Tile tile in Tiles)
            tile.Draw(spriteBatch, DrawOffset);

        if (SelectedPiece != null)
        {
            foreach (Tile tile in SelectedPiece.ReachableTiles)
            {
                if (tile.HasPiece && tile.Piece.IsEnemyOf(SelectedPiece))
                    spriteBatch.Draw(ReachableTileEnemyTexture, tile.Position + DrawOffset, null, ReachableTileColor);
                else
                    spriteBatch.Draw(ReachableTileTexture, tile.Position + DrawOffset, null, ReachableTileColor);
            }
        }

        foreach (Piece piece in Pieces)
            piece.Draw(spriteBatch, DrawOffset);

        ImGui.Begin("Board");
        
        ImGui.SeparatorText("Info");
        ImGui.Text($"{(IsWhiteTurn ? "White" : "Black")} to move");
        ImGui.Text($"Selected piece: {SelectedPiece}");
        if (ImGui.Button("Update reachable tiles"))
            UpdatePiecesReachableTiles();
        
        ImGui.SeparatorText("Moves");
        if (ImGui.Button("Make all moves"))
            Moves.MakeAll();
        ImGui.SameLine();
        if (ImGui.Button("Unmake all moves"))
            Moves.UnmakeAll();

        for (int i = 0; i < Moves.Count; i++)
        {
            Move move = Moves[i];
            if (i % 2 == 1)
                ImGui.SameLine(50f);
            ImGui.Text($"{move}");
        }
        
        ImGui.End();
    }

    public void ResetTileSelection()
    {
        foreach (Tile tile in Tiles)
            tile.State = Tile.SelectionState.Default;
    }

    private void UpdatePiecesReachableTiles()
    {
        foreach (Piece piece in Pieces)
        {
            piece.ReachableTiles.Clear();
            piece.UpdateReachableTiles();
        }
    }

    public King GetKing(bool white) => white ? WhiteKing : BlackKing;

    public Tile this[int tileX, int tileY]
    {
        get
        {
            if (tileX < 0 || tileY < 0 || tileX >= TileSize || tileY >= TileSize)
                return null;
            
            return Tiles[tileX, tileY];
        }
    }

    public Tile this[Point tilePos] => this[tilePos.X, tilePos.Y];
}
