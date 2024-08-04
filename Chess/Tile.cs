using System;
using Chess.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Chess;

public class Tile
{
    public enum SelectionState
    {
        Default,
        RedSelection,
        GreenSelection
    }
    
    public const float Size = 110f;

    public static Color WhiteColor { get; } = new(0xF0, 0xD9, 0xB5);
    public static Color BlackColor { get; } = new(0xB5, 0x88, 0x63);
    public static Color RedWhiteColor { get; } = new(0xEB, 0x78, 0x63);
    public static Color RedBlackColor { get; } = new(0xE1, 0x68, 0x54);
    public static Color GreenWhiteColor { get; } = new(0xB8, 0xCF, 0x6A);
    public static Color GreenBlackColor { get; } = new(0xAE, 0xBF, 0x5B);

    public const string RowLetters = "abcdefgh";

    public bool IsWhite { get; }

    public Board Board { get; }
    public Piece Piece { get; set; }

    public Point TilePosition { get; }
    public Vector2 Position => TilePosition.ToVector2() * Size;
    public RectangleF Area => new(Position, Vector2.One * Size);
    public RectangleF ScreenArea
    {
        get
        {
            RectangleF area = Area;
            area.Position += Board.DrawOffset;
            return area;
        }
    }

    private SelectionState state = SelectionState.Default;
    public SelectionState State
    {
        get => state;
        set
        {
            state = state == value ? SelectionState.Default : value;
            DrawColor = state switch
            {
                SelectionState.Default => Color.Transparent,
                SelectionState.RedSelection => IsWhite ? RedWhiteColor : RedBlackColor,
                SelectionState.GreenSelection => IsWhite ? GreenWhiteColor : GreenBlackColor,
                _ => throw new ArgumentException($"Invalid {nameof(SelectionState)} set to {nameof(Tile)}: {state}")
            };
        }
    }

    public bool HasPiece => Piece != null;

    private Color DrawColor { get; set; } = Color.Transparent;

    public Tile(Board board, Point tilePosition, bool isWhite)
    {
        Board = board;
        TilePosition = tilePosition;
        IsWhite = isWhite;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        if (DrawColor == Color.Transparent)
            return;
        
        spriteBatch.DrawRectangle(offset + Position, new(Size, Size), DrawColor, Size * 0.5f);
    }

    public void Update(KeyboardStateExtended keyboard, MouseStateExtended mouse)
    {
        if (mouse.IsButtonReleased(MouseButton.Right) && ScreenArea.Contains(mouse.Position))
            State = keyboard.IsShiftDown() ? SelectionState.GreenSelection : SelectionState.RedSelection;
        else if (mouse.IsButtonReleased(MouseButton.Left) && ScreenArea.Contains(mouse.Position))
            Board.ResetTileSelection();
    }

    public override string ToString() => $"{RowLetters[TilePosition.X]}{TilePosition.Y + 1}";
}
