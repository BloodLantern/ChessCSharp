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

    private static readonly Color WhiteColor = new(0xED, 0xD6, 0xB0);
    private static readonly Color BlackColor = new(0xB8, 0x87, 0x62);
    private static readonly Color RedWhiteColor = new(0xEB, 0x78, 0x63);
    private static readonly Color RedBlackColor = new(0xE1, 0x68, 0x54);
    private static readonly Color GreenWhiteColor = new(0xB8, 0xCF, 0x6A);
    private static readonly Color GreenBlackColor = new(0xAE, 0xBF, 0x5B);

    public bool IsWhite { get; }
    public bool IsBlack => !IsWhite;

    public Board Board { get; }
    public Piece Piece { get; set; }

    public Point TilePosition { get; }
    public Vector2 Position => TilePosition.ToVector2() * Size;
    public RectangleF Area => new(Position, Vector2.One * Size);

    private SelectionState state = SelectionState.Default;
    public SelectionState State
    {
        get => state;
        set
        {
            state = value;
            drawColor = value switch
            {
                SelectionState.Default => IsWhite ? WhiteColor : BlackColor,
                SelectionState.RedSelection => IsWhite ? RedWhiteColor : RedBlackColor,
                SelectionState.GreenSelection => IsWhite ? GreenWhiteColor : GreenBlackColor,
                _ => throw new ArgumentException($"Invalid {nameof(SelectionState)} set to {nameof(Tile)}")
            };
        }
    }

    private Color drawColor;

    public Tile(Board board, Point tilePosition, bool isWhite)
    {
        Board = board;
        TilePosition = tilePosition;
        IsWhite = isWhite;
        drawColor = isWhite ? WhiteColor : BlackColor;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        => spriteBatch.DrawRectangle(offset + Position, new(Size, Size), drawColor, Size * 0.5f);

    public void Update(KeyboardStateExtended keyboard, MouseStateExtended mouse)
    {
        RectangleF screenArea = Area;
        screenArea.Position += Board.DrawOffset;
        
        if (mouse.IsButtonReleased(MouseButton.Right) && screenArea.Contains(mouse.Position))
            State = keyboard.IsShiftDown() ? SelectionState.GreenSelection : SelectionState.RedSelection;
        else if (mouse.IsButtonReleased(MouseButton.Left) && screenArea.Contains(mouse.Position))
            Board.ResetTileSelection();
    }
}
