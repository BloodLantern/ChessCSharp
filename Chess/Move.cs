using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Chess.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MonoGame.Utils;
using MonoGame.Utils.Extensions;

namespace Chess;

public class Move
{
    private static SoundEffect BaseSfx { get; set; }
    private static SoundEffect CaptureSfx { get; set; }
    private static SoundEffect CastleSfx { get; set; }
    private static SoundEffect CheckSfx { get; set; }
    private static SoundEffect PromoteSfx { get; set; }
    
    public Piece Piece { get; }
    
    public Tile Origin { get; }
    public Tile Destination { get; }

    private Board Board { get; }
    private Piece DestinationPiece { get; }
    
    private SoundEffect Sfx { get; }
    
    public bool Made { get; private set; }
    
    public bool FirstPieceMove { get; }
    
    public bool IsCapture { get; }
    public bool IsCastle { get; }
    public bool IsCheck { get; }
    public bool IsPromote { get; }

    public Move(Board board, Piece piece, Tile destination)
        : this(board, piece, piece.Tile, destination)
    {
    }

    public Move(Board board, Piece piece, Tile origin, Tile destination)
    {
        Board = board ?? throw new ArgumentNullException(nameof(board));
        Piece = piece ?? throw new ArgumentNullException(nameof(piece));
        Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));

        DestinationPiece = Destination.Piece;

        Sfx = BaseSfx;
        
        FirstPieceMove = !Piece.HasMoved;
        
        IsCapture = DestinationPiece != null;
        IsPromote = Piece is Pawn && Destination.TilePosition.Y is 0 or 7;

        // If after the move is made the piece can now capture the enemy king, consider the current move as putting the king in check
        // However, if the ally king is instead put in check, consider the current move as illegal
        Make(false, false);
        List<Tile> newReachableTiles = Piece.GetReachableTiles();
        if (newReachableTiles.Contains(Board.GetKing(!Piece.IsWhite).Tile))
            IsCheck = true;
        Unmake(false, false);

        if (IsCapture)
            Sfx = CaptureSfx;
        if (IsPromote)
            Sfx = PromoteSfx;
        // Castling
        if (IsCheck)
            Sfx = CheckSfx;
    }

    internal static void LoadContent(ContentManager content)
    {
        BaseSfx = content.Load<SoundEffect>("audio/move");
        CaptureSfx = content.Load<SoundEffect>("audio/capture");
        CastleSfx = content.Load<SoundEffect>("audio/castle");
        CheckSfx = content.Load<SoundEffect>("audio/move-check");
        PromoteSfx = content.Load<SoundEffect>("audio/promote");
    }

    public void Make(bool playAnimation, bool playSfx)
    {
        if (playSfx)
            Sfx.Play();
        
        if (!playAnimation)
            RemoveDestinationPiece();

        Piece.Tile = Destination;
        Piece.HasMoved = true;

        if (playAnimation)
            Coroutine.Start(AnimationRoutine(Origin, Destination));

        Made = true;
    }

    public void Unmake(bool playAnimation, bool playSfx)
    {
        if (playSfx)
            Sfx.Play();

        Piece.Tile = Origin;
        AddDestinationPiece();

        if (FirstPieceMove)
            Piece.HasMoved = false;

        if (playAnimation)
            Coroutine.Start(AnimationRoutine(Destination, Origin));

        Made = false;
    }

    private IEnumerator AnimationRoutine(Tile origin, Tile destination)
    {
        const float Duration = 0.4f;
        
        Stopwatch stopwatch = Stopwatch.StartNew();

        bool makingMove = origin == Origin;
        
        Vector2 halfTileSize = Vector2.One * Tile.Size * 0.5f;
        Vector2 start = origin.Position + halfTileSize;
        Vector2 end = destination.Position + halfTileSize;

        for (float timer = 0f; timer < Duration; timer += stopwatch.GetElapsedSeconds())
        {
            Piece.Position = Calc.EaseLerp(start, end, timer, Duration, Ease.Linear);
            
            yield return null;
        }

        if (makingMove)
            RemoveDestinationPiece();
        
        Piece.Position = destination.Position + halfTileSize;
    }

    private void RemoveDestinationPiece()
    {
        if (Destination.Piece == null)
            return;
        
        Board.Pieces.Remove(DestinationPiece);
        Destination.Piece = null;
    }

    private void AddDestinationPiece()
    {
        if (DestinationPiece == null)
            return;
        
        Board.Pieces.Add(DestinationPiece);
        Destination.Piece = DestinationPiece;
    }

    public override string ToString() => $"{Piece}{Destination}";
}
