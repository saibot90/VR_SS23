/// <summary>
/// A Score with correct and total value
/// </summary>
public class Score
{
    #region Variables
    
    public int Correct { get; set; }
    public int Total { get; set; }
    
    #endregion

    #region Constructors

    /// <summary>
    /// Constructor of a score object
    /// </summary>
    /// <param name="correct">Value of the correct numbers of the score</param>
    /// <param name="total">Value of the total numbers of the score</param>
    public Score(int correct, int total)
    {
        Correct = correct;
        Total = total;
    }
    
    #endregion
}
