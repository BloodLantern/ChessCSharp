using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess;

public class MoveList : IEnumerable<Move>
{
    private List<Move> List { get; } = [];

    public int Count => List.Count;
    
    public int CurrentIndex { get; private set; }
    public Move Current => List[CurrentIndex];

    public bool Modified => CurrentIndex == Count - 1;

    public void Add(Move move)
    {
        if (!Modified)
            CurrentIndex++;

        List.Add(move);
    }

    public void MakeOne(bool animated)
    {
        if (!Modified)
            return;
        
        foreach (Move move in List.Where(move => !move.Made))
        {
            move.Make(animated);
            break;
        }

        CurrentIndex++;
    }

    public void UnmakeOne(bool animated)
    {
        if (CurrentIndex == 0)
            return;

        for (int i = List.Count - 1; i >= 0; i--)
        {
            Move move = List[i];
            if (!move.Made)
                continue;
                
            move.Unmake(animated);
            break;
        }

        CurrentIndex--;
    }

    public void MakeAll()
    {
        foreach (Move move in List.Where(move => !move.Made))
            move.Make(false);
    }

    public void UnmakeAll()
    {
        for (int i = List.Count - 1; i >= 0; i--)
        {
            Move move = List[i];
            if (!move.Made)
                continue;
                
            move.Unmake(false);
        }
    }

    public IEnumerator<Move> GetEnumerator() => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
