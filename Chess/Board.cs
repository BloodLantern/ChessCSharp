using System.Collections.Generic;
using Chess.Pieces;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Chess;

public class Board
{
    public const int TileSize = 8;
    public const float Size = Tile.Size * TileSize;

    public Texture2D Texture { get; private set; }
    public Texture2D PiecesTexture { get; private set; }
    
    private SoundEffect MovePieceSfx { get; set; }

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
            
            Pieces.Add(new Rook(this, Tiles[0, black ? 0 : 7], !black));
            Pieces.Add(new Knight(this, Tiles[1, black ? 0 : 7], !black));
            Pieces.Add(new Bishop(this, Tiles[2, black ? 0 : 7], !black));
            Pieces.Add(new Queen(this, Tiles[3, black ? 0 : 7], !black));
            Pieces.Add(new King(this, Tiles[4, black ? 0 : 7], !black));
            Pieces.Add(new Bishop(this, Tiles[5, black ? 0 : 7], !black));
            Pieces.Add(new Knight(this, Tiles[6, black ? 0 : 7], !black));
            Pieces.Add(new Rook(this, Tiles[7, black ? 0 : 7], !black));
            
            for (int j = 0; j < TileSize; j++)
                Pieces.Add(new Pawn(this, Tiles[j, black ? 1 : 6], !black));
        }

        Moves.OnMoveAdded += _ => MovePieceSfx!.Play();
        Moves.OnMoveMade += _ => MovePieceSfx!.Play();
        Moves.OnMoveUnmade += _ => MovePieceSfx!.Play();
    }

    public void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("board");
        PiecesTexture = content.Load<Texture2D>("pieces");

        MovePieceSfx = content.Load<SoundEffect>("audio/move-self");
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

                    if (SelectedPiece.Tile == tile)
                    {
                        SelectedPiece.ResetPosition();
                    }
                    else
                    {
                        Move move = new(this, SelectedPiece, tile);
                        move.Make(false);
                        Moves.Add(move);
                    }
                }

                SelectedPiece = null;
            }
        }

        if (keyboard.IsKeyPressed(Keys.Left))
            Moves.UnmakeOne(true);
        else if (keyboard.IsKeyPressed(Keys.Right))
            Moves.MakeOne(true);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, DrawOffset, Color.White);
        
        foreach (Tile tile in Tiles)
            tile.Draw(spriteBatch, DrawOffset);

        foreach (Piece piece in Pieces)
            piece.Draw(spriteBatch, DrawOffset);

        ImGui.Begin("Moves");
        
        if (ImGui.Button("Make all moves"))
            Moves.MakeAll();
        ImGui.SameLine();
        if (ImGui.Button("Unmake all moves"))
            Moves.UnmakeAll();

        foreach (Move move in Moves)
        {
            ImGui.Text($"{move}");
        }
        
        ImGui.End();
    }

    public void ResetTileSelection()
    {
        foreach (Tile tile in Tiles)
            tile.State = Tile.SelectionState.Default;
    }
}
