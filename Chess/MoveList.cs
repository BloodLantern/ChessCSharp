using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess;

public class MoveList : IEnumerable<Move>
{
    public delegate void MoveAction(Move move);

    private List<Move> List { get; } = [];

    public int Count => List.Count;

    public int CurrentIndex { get; private set; } = -1;
    public Move Current => List[CurrentIndex];

    public bool CurrentIsLastMove => CurrentIndex == Count - 1;

    public event MoveAction OnMoveAdded;
    public event MoveAction OnMoveMade;
    public event MoveAction OnMoveUnmade;

    public void Add(Move move)
    {
        if (CurrentIsLastMove)
            CurrentIndex++;

        List.Add(move);
        OnMoveAdded?.Invoke(move);
    }

    public void MakeOne(bool animated)
    {
        if (CurrentIsLastMove)
            return;
        
        foreach (Move move in List.Where(move => !move.Made))
        {
            move.Make(animated);
            OnMoveMade?.Invoke(move);
            break;
        }

        CurrentIndex++;
    }

    public void UnmakeOne(bool animated)
    {
        if (CurrentIndex == -1)
            return;

        for (int i = List.Count - 1; i >= 0; i--)
        {
            Move move = List[i];
            if (!move.Made)
                continue;
                
            move.Unmake(animated);
            OnMoveUnmade?.Invoke(move);
            break;
        }

        CurrentIndex--;
    }

    public void MakeAll()
    {
        foreach (Move move in List.Where(move => !move.Made))
        {
            move.Make(false);
            OnMoveMade?.Invoke(move);
        }
    }

    public void UnmakeAll()
    {
        for (int i = List.Count - 1; i >= 0; i--)
        {
            Move move = List[i];
            if (!move.Made)
                continue;
                
            move.Unmake(false);
            OnMoveUnmade?.Invoke(move);
        }
    }

    public IEnumerator<Move> GetEnumerator() => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Move this[int i] => List[i];
}
