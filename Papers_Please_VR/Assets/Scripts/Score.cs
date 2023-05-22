using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public int Correct { get; set; }
    public int Total { get; set; }

    public Score()
    {
        Correct = 0;
        Total = 0;
    }

    public Score(int correct, int total)
    {
        Correct = correct;
        Total = total;
    }

    public void Reset ()
    {
        Correct = 0;
        Total = 0;
    }
}
