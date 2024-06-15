﻿using System;
using System.Collections;
using System.Diagnostics;
using Chess.Pieces;
using Microsoft.Xna.Framework;
using MonoGame.Utils;
using MonoGame.Utils.Extensions;

namespace Chess;

public class Move
{
    public Piece Piece { get; }
    
    public Tile Origin { get; }
    public Tile Destination { get; }

    private Board Board { get; }
    private Piece DestinationPiece { get; }
    
    public bool Made { get; private set; }
    
    public bool FirstPieceMove { get; }

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
        FirstPieceMove = !piece.HasMoved;
    }

    public void Make(bool animated)
    {
        if (!animated)
            RemoveDestinationPiece();

        Piece.Tile = Destination;
        Piece.HasMoved = true;

        if (animated)
            Coroutine.Start(AnimationRoutine(Origin, Destination));

        Made = true;
    }

    public void Unmake(bool animated)
    {
        Piece.Tile = Origin;
        AddDestinationPiece();

        if (FirstPieceMove)
            Piece.HasMoved = false;

        if (animated)
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
